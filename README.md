### An ASP.NET framework bootstrap extensions package which helps in managing IoC using DI containers (Autofac, Unity and ServiceCollection) and creating dynamic http handlers on a fly

Build | PivotalServices.AspNet.Bootstrap.Extensions |
--- | --- |
[![Build Status](https://dev.azure.com/ajaganathan-home/pivotal_aspnet_bootstrap_extensions/_apis/build/status/alfusinigoj.pivotal_aspnet_bootstrap_extensions?branchName=master)](https://dev.azure.com/ajaganathan-home/pivotal-aspnet-bootstrap-extensions/_build/latest?definitionId=2&branchName=master) | [![NuGet](https://img.shields.io/nuget/v/ivotalServices.AspNet.Bootstrap.Extension.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions) 

### Quick Links
- [Supported ASP.NET apps](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions#supported-aspnet-apps)
- [Salient features](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions#salient-features)
- [Package feature (IoC)](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions/#features-ioc)
- [Package feature (Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions/#feature-dynamic-handlers)
- [Sample Implementations](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions/tree/master/samples) 

### Supported ASP.NET apps
- WebAPI
- MVC
- WebForms
- Other types like (.asmx, .ashx)
- All the above with Unity
- All the above with Autofac

### Salient features
- Supports IoC using Autofac and Unity apart from native Microsoft ServiceCollection
- Provision for injecting http pipeline handlers on the fly
- Explicit access to any of the injected dependencies across your code. For e.g to access `IConfiguration` you can access it using `DependencyContainer.GetService<IConfiguration>()`. You can also access them via constructor injection which absolutely depends on the IoC framework and application.
- Real samples are available [here](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions/tree/master/samples) 

### Packages
- Extensions package - [PivotalServices.AspNet.Bootstrap.Extensions](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions)
 
### Prerequisites
- Application is upgraded to ASP.NET framework 4.6.2 or above

### Feature (Ioc)
- Install package [PivotalServices.AspNet.Bootstrap.Extensions](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions)
- Can add more `Actions` exposed where you can configure; `application configurations`, `inject services` and even modify `logging configurations` as needed.
  
```c#
    using PivotalServices.AspNet.Bootstrap.Extensions

    protected void Application_Start()
    {
		AppBuilder
			.Instance
			.ConfigureAppConfiguration((hostBuilder, configBuilder) =>
			{
				//Add additional configurations here
			})
			.ConfigureServices((hostBuilder, services) =>
			{
				//Add additional services here
			})
			.ConfigureLogging((hostBuilder, logBuilder) =>
			{
				//configure custome logging here
			}) 
			.Build()
			.Start();
	}
```

- If the application uses `Unity` as its dependency container, you can hook them together as below
      
```c#
    using PivotalServices.AspNet.Bootstrap.Extensions

    protected void Application_Start()
    {
		AppBuilder
		.Instance
		.ConfigureIoC(
		() => {
			return new Unity.AspNet.WebApi.UnityDependencyResolver(UnityConfig.Container);
		},
		() => {
			return new Unity.AspNet.Mvc.UnityDependencyResolver(UnityConfig.Container);
		},
		(services) => {
			UnityConfig.Container.BuildServiceProvider(services);
			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new Unity.AspNet.Mvc.UnityFilterAttributeFilterProvider(UnityConfig.Container));
		})
		.Build()
		.Start();
	}
```
- Sample `UnityConfig` class 
    
```c#
    using System;
    using Unity;
        
    namespace Foo
    {
        public static class UnityConfig
        {
            private static Lazy<IUnityContainer> container =
                new Lazy<IUnityContainer>(() =>
                {
                    var container = new UnityContainer();
                    RegisterTypes(container);
                    return container;
                });
        
            public static IUnityContainer Container => container.Value;
        
            public static void RegisterTypes(IUnityContainer container)
            {
                // TODO: Register your type's mappings here.
                //container.RegisterType<ITestClass, TestClass>();
            }
        }
    }
```

- If the application uses `Autofac` as its dependency container, you can hook them together as below
  
```c#
    using PivotalServices.AspNet.Bootstrap.Extensions

    protected void Application_Start()
    {
		AppBuilder
			.Instance
			.ConfigureIoC(
			() => {
				return new AutofacWebApiDependencyResolver(AutofacConfig.Container);
			},
			() => {
				return new AutofacDependencyResolver(AutofacConfig.Container);
			},
			(services) => {
				AutofacConfig.Builder.Populate(services);
			})
			.Build()
			.Start();
	}
```
- Sample `AutofacConfig` class 
        
```c#
    using Autofac;
    using System;
        
    namespace Foo
    {
        public class AutofacConfig
        {
            private static Lazy<IContainer> container =
                new Lazy<IContainer>(() =>
                {
                    RegisterTypes();
                    return Builder.Build();
                });
        
            public static IContainer Container => container.Value;
        
            public static ContainerBuilder Builder { get; private set; } = new ContainerBuilder();
        
            public static void RegisterTypes()
            {
                // TODO: Register your type's mappings here.
                //Builder.RegisterType<IProductRepository, ProductRepository>();
            }
        }
    }
```
#### Feature (Dynamic Handlers)
- Provision to inject any custom http handler using an implementation of abstract `DynamicHttpHandlerBase`
- Below is a sample api handler `FooHandler` below which responds to a `GET` operation with request path `/foo`. 

```c#
    using Microsoft.Extensions.Logging;
    using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
    using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
    using System.Web;

    namespace Bar
    {
        public class FooHandler : DynamicHttpHandlerBase
		{
			public FooHandler(ILogger<FooHandler> logger)
				: base(logger)
			{
			}

			public override string Path => "/foo";

			public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.PostAuthorizeRequestAsync;

			public override void HandleRequest(HttpContextBase context)
			{
				switch (context.Request.HttpMethod)
				{
					case "GET":
						PerformGet(context);
						break;
					default:
						logger.LogWarning($"No action found for method {context.Request.HttpMethod}");
						break;
				}
			}

			private void PerformGet(HttpContextBase context)
			{
				context.Response.Headers.Set("Content-Type", "application/json");
				context.Response.Write(new { Name = "FooHandler", Method = "GET" });
			}
		}
    }
```
- Override method `IsEnabledAsync` Default is `true`, but access can be overriden based on permissions here
- Override method `ContinueNextAsync` Should continue processing the request after this handler, default is `false`
- Override property `Path`, request path to which the handler to respond
- Override property `ApplicationEvent`, what kind of application event to handle

- Inject the above handler into the pipeline, as in the code below

```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
	using Bar;
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddDynamicHttpHandler<FooHandler>()
                .Build()
                .Start();
    }
```

### Ongoing development packages in MyGet

Feed | PivotalServices.AspNet.Bootstrap.Extensions |
--- | --- |
[V3](https://www.myget.org/F/ajaganathan/api/v3/index.json) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions) 

### Issues
- Kindly raise any issues under [GitHub Issues](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_extensions/issues)

### Contributions are welcome!
