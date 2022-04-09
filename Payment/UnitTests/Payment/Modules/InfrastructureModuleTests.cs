using Domain.Operations;
using Domain.Repository;
using Domain.Repository.Reader;
using Domain.Repository.Writer;
using Domain.Validators;
using FluentAssertions;
using Infrastructure.Repository;
using Infrastructure.Repository.Reader;
using Infrastructure.Repository.Writer;
using Payment.Operations;
using Payment.ValidationHandlers;
using Payment.Validators;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Modules
{
    public class InfrastructureModuleTests : IClassFixture<DependencyInjectionClassFixture>
    {
        private readonly DependencyInjectionClassFixture _fixture;

        public InfrastructureModuleTests(DependencyInjectionClassFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetService_ShouldResolve_IConnectionFactory()
        {
            var resolved = _fixture.Provider.GetService(typeof(IConnectionFactory))
                                .Should().BeOfType<ConnectionFactory>().Subject;

            resolved.Connection.Should().Be("teste");
        }

        [Fact]
        public void GetService_ShouldResolve_SameInstanceOfConnectionFactory()
        {
            ConnectionFactory instance1;
            ConnectionFactory instance2;
            
            instance1 = _fixture.Provider.GetService(typeof(IConnectionFactory)).As<ConnectionFactory>();
            instance2 = _fixture.Provider.GetService(typeof(IConnectionFactory)).As<ConnectionFactory>();

            var hash1 = instance1.GetHashCode();
            var hash2 = instance2.GetHashCode();
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void GetService_ShouldResolve_IPersonReader()
        {
            var resolved = _fixture.Provider.GetService(typeof(IPersonReader));

            resolved.Should().BeOfType<PersonReader>();
        }

        [Fact]
        public void GetService_ShouldResolve_IAccountReader()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountReader));

            resolved.Should().BeOfType<AccountReader>();
        }

        [Fact]
        public void GetService_ShouldResolve_IAccountWriter()
        {
            var resolved = _fixture.Provider.GetService(typeof(IAccountWriter));

            resolved.Should().BeOfType<AccountWriter>();
        }

        [Fact]
        public void GetService_ShouldResolve_ITransactionWriter()
        {
            var resolved = _fixture.Provider.GetService(typeof(ITransactionWriter));

            resolved.Should().BeOfType<TransactionWriter>();
        }

        [Fact]
        public void GetService_ShouldResolve_ITransactionReader()
        {
            var resolved = _fixture.Provider.GetService(typeof(ITransactionReader));

            resolved.Should().BeOfType<TransactionReader>();
        }
    }
}
