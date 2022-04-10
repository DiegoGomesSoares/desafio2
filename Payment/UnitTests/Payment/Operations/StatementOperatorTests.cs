using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Models.Requests;
using Domain.Operations;
using FluentAssertions;
using NSubstitute;
using Payment.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Operations
{
    public class StatementOperatorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(StatementOperator).GetConstructors());
        }

        [Fact]
        public void Sut_ShouldImplement_IStatementOperator()
        {
            typeof(StatementOperator).Should().BeAssignableTo<IStatementOperator>();
        }

        [Theory, AutoNSubstituteData]
        public async Task GetStatementAsync_ShouldReturnStatementUntilLastTimeOfEndDate(
            StatementRequest model,
            IEnumerable<Transacao> transactions,
            StatementOperator sut) 
        {
            model.StartDate = "01/20/2022";
            model.EndDate = "01/25/2022";
            model.Size = 10;
            model.Page = 1;
            var countTotalTransaction = 100;
            sut.TransactionReader.GetTotalCountAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                                    .Returns(countTotalTransaction);
            sut.TransactionReader
                        .GetAllPaginatedAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>())
                            .Returns(transactions);

            var actual = await sut.GetStatementAsync(model);

            actual.IdConta.Should().Be(model.AccountId);
            actual.Saldo.Should().Be(model.Balance);

            var expectedTotalPages = (int)Math.Ceiling((double)countTotalTransaction / model.Size);
            actual.TotalPaginas.Should().Be(expectedTotalPages);

            actual.PaginaIndex.Should().Be(model.Page);
            actual.TotalTransacao.Should().Be(countTotalTransaction);
            actual.Transacoes.Should().NotBeEmpty();


            var expectedStartDate = GetExpectedStartDate(model.StartDate);
            var expectedEndDate = GetExpectedEndDate(model.EndDate);

            await sut.TransactionReader.Received().GetTotalCountAsync(model.AccountId, expectedStartDate, expectedEndDate);
            await sut.TransactionReader.Received()
                        .GetAllPaginatedAsync(model.AccountId, expectedStartDate, expectedEndDate, model.Page, model.Size);

        }

        [Theory]
        [AutoInlineData("", "")]
        [AutoInlineData(" ", " ")]
        [AutoInlineData(null, null)]
        [AutoInlineData("25/02/2022", "26/02/2022")]
        [AutoInlineData("1/02/2022", "1/03/2022")]
        public async Task GetStatementAsync_WhenPeriodIsInvalid_ReturnLast1DayStamentUntilNow(
            string starDate,
            string endDate,
            StatementRequest model,
            IEnumerable<Transacao> transactions,
            StatementOperator sut)
        {
            model.StartDate = starDate;
            model.EndDate = endDate;
            model.Size = 10;
            model.Page = 1;
            var countTotalTransaction = 100;
            sut.TransactionReader.GetTotalCountAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                                    .Returns(countTotalTransaction);
            sut.TransactionReader
                        .GetAllPaginatedAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>())
                            .Returns(transactions);

            var actual = await sut.GetStatementAsync(model);

            actual.IdConta.Should().Be(model.AccountId);
            actual.Saldo.Should().Be(model.Balance);

            var expectedTotalPages = (int)Math.Ceiling((double)countTotalTransaction / model.Size);
            actual.TotalPaginas.Should().Be(expectedTotalPages);

            actual.PaginaIndex.Should().Be(model.Page);
            actual.TotalTransacao.Should().Be(countTotalTransaction);
            actual.Transacoes.Should().NotBeEmpty();


            var expectedStartDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd h:mm:ss tt");            
            var expectedEndDate = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");

            await sut.TransactionReader.Received().GetTotalCountAsync(model.AccountId, expectedStartDate, expectedEndDate);
            await sut.TransactionReader.Received()
                        .GetAllPaginatedAsync(model.AccountId, expectedStartDate, expectedEndDate, model.Page, model.Size);

        }

        private static string GetExpectedEndDate(string endDate)
        {
            DateTime.TryParseExact(endDate, "MM/dd/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime endDateConverted);

            return endDateConverted.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd h:mm:ss tt");
        }

        private static string GetExpectedStartDate(string startDate)
        {
            DateTime.TryParseExact(startDate, "MM/dd/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime startConverted);
            return startConverted.ToString("yyyy-MM-dd h:mm:ss tt");
        }
    }
}
