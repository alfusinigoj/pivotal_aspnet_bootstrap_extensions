﻿using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Handlers
{
    internal interface IDynamicHttpHandler
    {
        string Path { get; }
        DynamicHttpHandlerEvent ApplicationEvent { get; }
        void HandleRequest(HttpContextBase context);
        bool IsEnabled(HttpContextBase context);
        bool ContinueNext(HttpContextBase context);
        void RegisterEvent(HttpApplication application);
    }
}