using Domain.Operations;
using Domain.Validators;
using Microsoft.Extensions.DependencyInjection;
using Payment.Operations;
using Payment.ValidationHandlers;
using Payment.Validators;

namespace Payment.Modules
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(
          this IServiceCollection services)
        {
            services.AddTransient<IAccountCreateValidator, AccountCreateValidator>()
                    .AddTransient<IAccountIdValidator, AccountIdValidator>()
                    .AddTransient<IAccountTransactionValidator, AccountTransactionValidator>()
                    .AddTransient<IValidationHandler, ValidationHandler>()
                    .AddTransient<IAccountOperator, AccountOperator>()
                    .AddTransient<IStatementOperator, StatementOperator>();

            return services;
        }
    }
}
