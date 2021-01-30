using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Battleship.Tests
{
    [TestClass]
    public class StartupTests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IServiceCollection> _serviceCollection;
        private Mock<IApplicationBuilder> _applicationBuilder;
        private Mock<IWebHostEnvironment> _webHostEnvironment;
        private Startup _startup;

        [TestInitialize]
        public void Initialise()
        {
            _configuration = new Mock<IConfiguration>();
            _serviceCollection = new Mock<IServiceCollection>();
            _applicationBuilder = new Mock<IApplicationBuilder>();
            _webHostEnvironment = new Mock<IWebHostEnvironment>();
            _startup = new Startup(_configuration.Object);
        }

        [TestMethod]
        public void ConfigureServices_Should_ConfigureServices()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.IsNotNull(webHost);
        }
    }
}
