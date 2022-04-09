using Domain.Operations;
using Domain.Validators;
using FluentAssertions;
using Payment.Operations;
using Payment.ValidationHandlers;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Modules
{
    public class ApplicationModuleTests : IClassFixture<DependencyInjectionClassFixture>
    {
        private readonly DependencyInjectionClassFixture _fixture;

        public ApplicationModuleTests(DependencyInjectionClassFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetService_ShouldResolve_IAccountCreateValidator()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountCreateValidator));

            resolved.Should().BeOfType<AccountCreateValidator>();
        }

        [Fact]
        public void GetService_ShouldResolve_IIAccountIdValidator()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountIdValidator));

            resolved.Should().BeOfType<AccountIdValidator>();
        }

        [Fact]
        public void GetService_ShouldResolve_IAccountTransactionValidator()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountTransactionValidator));

            resolved.Should().BeOfType<AccountTransactionValidator>();
        }
        [Fact]
        public void GetService_ShouldResolve_IValidationHandler()
        {
            var resolved = _fixture.Provider.GetService(typeof(IValidationHandler));

            resolved.Should().BeOfType<ValidationHandler>();
        }
        [Fact]
        public void GetService_ShouldResolve_IAccountOperator()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountOperator));

            resolved.Should().BeOfType<AccountOperator>();
        }
        [Fact]
        public void GetService_ShouldResolve_IStatementOperator()
        {
            var resolved = _fixture.Provider.GetService(typeof(IStatementOperator));

            resolved.Should().BeOfType<StatementOperator>();
        }
    }
}
