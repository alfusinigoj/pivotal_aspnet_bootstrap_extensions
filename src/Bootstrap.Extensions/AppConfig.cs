﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PivotalServices.AspNet.Bootstrap.Extensions
{
    internal sealed class AppConfig
    {
        private AppConfig() { }
        static IHost host;

        public static void Configure(List<Action<HostBuilderContext, IConfigurationBuilder>> configureAppConfigurationDelegates,
                                     List<Action<HostBuilderContext, IServiceCollection>> configureServicesDelegates,
                                     List<Action<HostBuilderContext, ILoggingBuilder>> configureLoggingDelegates,
                                     Action<IServiceCollection> configureIoCDelegate,
                                     Dictionary<string, string> inMemoryConfigurationStore)
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    configBuilder.AddInMemoryConfiguration(inMemoryConfigurationStore);

                    foreach (var configureAppConfigurationDelegate in configureAppConfigurationDelegates)
                    {
                        configureAppConfigurationDelegate?.Invoke(builderContext, configBuilder);
                    }
                })
                .ConfigureServices((builderContext, services) =>
                {
                    services.AddOptions();
                    services.AddLogging((builder) =>
                    {
                        builder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                        builder.AddConsole();
                        builder.AddDebug();
                    });

                    foreach (var configureServicesDelegate in configureServicesDelegates)
                    {
                        configureServicesDelegate?.Invoke(builderContext, services);
                    }

                    configureIoCDelegate?.Invoke(services);
                })
                .ConfigureLogging((builder, logBuilder) =>
                {
                    foreach (var configureLoggingDelegate in configureLoggingDelegates)
                    {
                        configureLoggingDelegate?.Invoke(builder, logBuilder);
                    }
                })
                .Build();
        }

        public static T GetService<T>()
        {
            if (host == null || host.Services == null)
                throw new ApplicationException("AppBuilder is not setup at the startup or built properly!");

            return host.Services.GetService<T>();
        }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (host == null || host.Services == null)
                    throw new ApplicationException("AppBuilder is not setup at the startup or built properly!");

                return host.Services;
            }
        }
    }
}
