using AutoFixture.Idioms;
using Domain.Operations;
using FluentAssertions;
using Payment.Operations;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Operations
{
    public class AccountOperatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountOperator).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_IAccountOperator()
        {
            typeof(AccountOperator).Should().BeAssignableTo<IAccountOperator>();
        }

    }
}
