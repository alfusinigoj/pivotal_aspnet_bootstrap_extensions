using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using System;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Tests.Handlers
{
    public class DynamicHttpHandlerBaseTests
    {
        Mock<ILogger<DynamicHttpHandlerSpy>> logger;
        public DynamicHttpHandlerBaseTests()
        {
            logger = new Mock<ILogger<DynamicHttpHandlerSpy>>();
        }

        [Fact]
        public void Test_ContinueNextShouldReturnFalseByDefault()
        {
            var handler = new DynamicHttpHandlerSpy(logger.Object);
            Assert.False(handler.ContinueNext(null));
        }

        [Fact]
        public void Test_IsEnabledShouldReturnTrueByDefault()
        {
            var handler = new DynamicHttpHandlerSpy(logger.Object);
            Assert.True(handler.IsEnabled(null));
        }
    }

    public class DynamicHttpHandlerSpy : DynamicHttpHandlerBase
    {
        public DynamicHttpHandlerSpy(ILogger<DynamicHttpHandlerSpy> logger) : base(logger)
        {
        }

        public override string Path => throw new NotImplementedException();

        public override DynamicHttpHandlerEvent ApplicationEvent => throw new NotImplementedException();

        public override void HandleRequest(HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}
