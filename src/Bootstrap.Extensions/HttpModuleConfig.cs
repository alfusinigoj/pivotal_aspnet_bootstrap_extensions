using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using PivotalServices.AspNet.Bootstrap.Extensions;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using System;
using System.Collections.Generic;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(HttpModuleConfig), "ConfigureModules")]

namespace PivotalServices.AspNet.Bootstrap.Extensions
{
    public class HttpModuleConfig
    {
        public static void ConfigureModules()
        {
            foreach (var moduleType in GetModuleTypes())
            {
                DynamicModuleUtility.RegisterModule(moduleType);
            }
        }

        public static IEnumerable<Type> GetModuleTypes()
        {
            return new List<Type>
            {
                typeof(DynamicHttpHandlerModule),
            };
        }
    }
}
