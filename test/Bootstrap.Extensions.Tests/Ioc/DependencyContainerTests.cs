using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PivotalServices.Bootstrap.Extensions.Tests.Ioc
{
    public class DependencyContainerTests
    {
        [Fact]
        public void Test_DependencyContainer_GetService_ProvideAValidErrorMessageIfAppbuilderIsNotInstantiated()
        {
            Assert.Throws<ApplicationException>(() => DependencyContainer.GetService<IAsyncDisposable>());
        }
    }
}
