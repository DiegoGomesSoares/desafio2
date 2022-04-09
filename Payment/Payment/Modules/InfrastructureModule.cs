using Domain.Repository;
using Domain.Repository.Reader;
using Domain.Repository.Writer;
using Infrastructure.Repository;
using Infrastructure.Repository.Reader;
using Infrastructure.Repository.Writer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Payment.Modules
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory>(p =>
            {
                var connectionString = configuration.GetValue<string>("PaymentDb");

                return new ConnectionFactory(connectionString);
            });

            services.AddTransient<IPersonReader, PersonReader>()
                    .AddTransient<IAccountReader, AccountReader>()
                    .AddTransient<IAccountWriter, AccountWriter>()                    
                    .AddTransient<ITransactionWriter, TransactionWriter>()
                    .AddTransient<ITransactionReader, TransactionReader>();

            return services;
        }
    }
}
