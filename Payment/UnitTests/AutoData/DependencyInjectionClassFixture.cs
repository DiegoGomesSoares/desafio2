using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment;

namespace UnitTests.AutoData
{
    public class DependencyInjectionClassFixture
    {
        public ServiceProvider Provider { get; }

        public DependencyInjectionClassFixture()
        {
            var services = new ServiceCollection();

            var startup = new Startup(GetConfiguration());
            startup.ConfigureServices(services);

            Provider = services.BuildServiceProvider();
        }

        public static IConfiguration GetConfiguration()
        {
            var configuration =
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.tests.json")
                    .Build();

            return configuration;
        }
    }
}
