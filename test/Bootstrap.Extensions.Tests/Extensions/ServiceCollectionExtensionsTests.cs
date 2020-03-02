using Microsoft.Extensions.DependencyInjection;
using PivotalServices.AspNet.Bootstrap.Extensions.Testing;
using System.Linq;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PivotalServices.AspNet.Bootstrap.Extensions.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Test_AddControllers_AddsOnlyClassesEndsWithControllersAndImplementationOfIController()
        {
            var services = new ServiceCollection();

            TestProxy.AddControllersProxy(services);

            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "PivotalServices.AspNet.Bootstrap.Extensions.Tests.TestApiController");
            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "PivotalServices.AspNet.Bootstrap.Extensions.Tests.TestMvcController");
            Assert.True(!services.Any((desc) => desc?.ImplementationType?.FullName == "PivotalServices.AspNet.Bootstrap.Extensions.Tests.TestController3"));
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete



