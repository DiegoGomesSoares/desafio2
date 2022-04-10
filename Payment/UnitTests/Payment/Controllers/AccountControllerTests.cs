using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Payment.Controllers;
using System;
using System.Net;
using System.Threading.Tasks;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Controllers
{
    public class AccountControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountController).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task Create_WhenIsNotValid_ShouldReturnBadRequestWithErrorMessage(
            AccountRequest model,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessage = "error";

            sut.AccountCreateValidator
                .ValidateAsync(Arg.Any<int>(), Arg.Any<AccountTypeEnum>())
                .Returns(validationResult);

            var actual = await sut.Create(model) as BadRequestObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().BeEquivalentTo(validationResult.ErrorMessage);

            await sut.AccountCreateValidator
                    .Received().ValidateAsync(model.PersonId, model.Type);

            await sut.AccountWriter.DidNotReceive().CreateAsync(Arg.Any<Conta>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Create_WhenExecuteCorrectly_ShouldReturnHttpStatusCodeOkWithResponse(
            AccountRequest model,
            ValidateResultModel validationResult,
            int accountId,
            AccountController sut)
        {
            validationResult.IsValid = true;
            var expectedBalance = 0;
            sut.AccountCreateValidator
                .ValidateAsync(Arg.Any<int>(), Arg.Any<AccountTypeEnum>())
                .Returns(validationResult);

            sut.AccountWriter.CreateAsync(Arg.Do<Conta>(x => 
            {
                x.FlagAtivo.Should().Be(true);
                x.IdPessoa.Should().Be(model.PersonId);
                x.LimiteSaqueDiario.Should().Be(model.DailyCashOutLimit);
                x.TipoConta.Should().Be((short)model.Type);
                x.Saldo.Should().Be(expectedBalance);
                x.DataCriacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(100));
            })).Returns(accountId);

            var actual = await sut.Create(model) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var accountResponse = (AccountResponse)actual.Value;
            accountResponse.IdConta.Should().Be(accountId);
            accountResponse.Saldo.Should().Be(expectedBalance);
            accountResponse.DataCriacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(100));

            await sut.AccountCreateValidator
                    .Received().ValidateAsync(model.PersonId, model.Type);

            await sut.AccountWriter.Received().CreateAsync(Arg.Any<Conta>());
        }

        [Theory, AutoNSubstituteData]
        public async Task GetBalance_WhenIsNotValid_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupInvalidValidator(validationResult, sut);

            var actual = await sut.GetBalance(accountId) as BadRequestObjectResult;

            AssertInValidValidator(actual, sut, accountId, validationResult);

            await sut.AccountReader.DidNotReceive().GetAccountByIdAsync(Arg.Any<int>());
        }

        [Theory, AutoNSubstituteData]
        public async Task GetBalance_WhenAccountWasNotFounded_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupAccountNotFound(sut, validationResult);

            var actual = await sut.GetBalance(accountId) as BadRequestObjectResult;

            await AssertAccountNotFound(actual, sut, accountId);
        }

        [Theory, AutoNSubstituteData]
        public async Task GetBalance_WhenExecuteCorrectly_ShouldReturnHttpStatusCodeOkWithResponse(
            int accountId,
            ValidateResultModel validationResult,
            Conta account,
            AccountController sut)
        {
            validationResult.IsValid = true;

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);

            var actual = await sut.GetBalance(accountId) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var getBalanceResponse = (GetBalanceResponse)actual.Value;
            getBalanceResponse.Saldo.Should().Be(account.Saldo);

            sut.AccountIdValidator.Received().Validate(accountId);
            await sut.AccountReader.Received().GetAccountByIdAsync(accountId);
        }

        [Theory, AutoNSubstituteData]
        public async Task Block_WhenIsNotValid_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessage = "error";

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            var actual = await sut.Block(accountId) as BadRequestObjectResult;

            AssertInValidValidator(actual, sut, accountId, validationResult);

            await sut.AccountReader.DidNotReceive().GetAccountByIdAsync(Arg.Any<int>());
            await sut.AccountWriter.DidNotReceive().BlockAccount(Arg.Any<int>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Block_WhenAccountWasNotFounded_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupAccountNotFound(sut, validationResult);

            var actual = await sut.Block(accountId) as BadRequestObjectResult;

            await AssertAccountNotFound(actual, sut, accountId);

            await sut.AccountWriter.DidNotReceive().BlockAccount(Arg.Any<int>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Block_WhenAccountIsNotEnabled_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            Conta account,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupBlockedAccount(sut, account, validationResult);

            var actual = await sut.Block(accountId) as BadRequestObjectResult;

            await AssertBlockedAccount(actual, sut, accountId);
            await sut.AccountWriter.DidNotReceive().BlockAccount(Arg.Any<int>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Block_WhenExecuteCorrectly_ShouldReturnOk(
            int accountId,
            Conta account,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            validationResult.IsValid = true;
            account.FlagAtivo = true;

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);

            var actual = await sut.Block(accountId) as OkResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            sut.AccountIdValidator.Received().Validate(accountId);
            await sut.AccountReader.Received().GetAccountByIdAsync(accountId);
            await sut.AccountWriter.Received().BlockAccount(accountId);
        }

        [Theory, AutoNSubstituteData]
        public async Task GetStatement_WhenIsNotValid_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupInvalidValidator(validationResult, sut);

            var actual = await sut.GetStatement(accountId, null, null, 10, 1) as BadRequestObjectResult;

            AssertInValidValidator(actual, sut, accountId, validationResult);
            
            await sut.AccountReader.DidNotReceive().GetAccountByIdAsync(Arg.Any<int>());
            await sut.StatementOperator.DidNotReceive().GetStatementAsync(Arg.Any<StatementRequest>());
        }

        [Theory, AutoNSubstituteData]
        public async Task GetStatement_WhenAccountWasNotFounded_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupAccountNotFound(sut, validationResult);

            var actual = await sut.GetStatement(accountId, null, null, 10, 1) as BadRequestObjectResult;

            await AssertAccountNotFound(actual, sut, accountId);

            await sut.StatementOperator.DidNotReceive().GetStatementAsync(Arg.Any<StatementRequest>());
        }

        [Theory, AutoNSubstituteData]
        public async Task GetStatement_WhenAccountIsNotEnabled_ShouldReturnBadRequestWithErrorMessage(
            int accountId,
            Conta account,
            ValidateResultModel validationResult,
            AccountController sut)
        {
            SetupBlockedAccount(sut, account, validationResult);

            var actual = await sut.GetStatement(accountId, null, null, 10, 1) as BadRequestObjectResult;

            await AssertBlockedAccount(actual, sut, accountId);
            await sut.StatementOperator.DidNotReceive().GetStatementAsync(Arg.Any<StatementRequest>());
        }

        [Theory, AutoNSubstituteData]
        public async Task GetStatement_WhenExecuteCorrectly_ShouldReturnOk(
            int accountId,
            int pageSize,
            int page,
            string startDate,
            string endDate,
            Conta account,
            ValidateResultModel validationResult,
            StatementResponse response,
            AccountController sut)
        {
            validationResult.IsValid = true;
            account.FlagAtivo = true;

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);

            sut.StatementOperator.GetStatementAsync(Arg.Do<StatementRequest>(x => 
            {
                x.AccountId.Should().Be(accountId);
                x.Balance.Should().Be(account.Saldo);
                x.StartDate.Should().Be(startDate);
                x.EndDate.Should().Be(endDate);
                x.Size.Should().Be(pageSize);
                x.Page.Should().Be(page);
            }))
                .Returns(response);

            var actual = await sut.GetStatement(accountId, startDate, endDate, pageSize, page) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var statement = (StatementResponse)actual.Value;
            statement.Should().Be(response);

            sut.AccountIdValidator.Received().Validate(accountId);
            await sut.AccountReader.Received().GetAccountByIdAsync(accountId);
            await sut.StatementOperator.Received().GetStatementAsync(Arg.Any<StatementRequest>());
        }

        private async Task AssertBlockedAccount(BadRequestObjectResult actual, AccountController sut, int accountId)
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var expectedMessage = "Account Is Already Blocked";
            actual.Value.Should().BeEquivalentTo(expectedMessage);

            sut.AccountIdValidator.Received().Validate(accountId);
            await sut.AccountReader.Received().GetAccountByIdAsync(accountId);
        }

        private static void SetupBlockedAccount(AccountController sut, Conta account, ValidateResultModel validationResult)
        {
            validationResult.IsValid = true;
            account.FlagAtivo = false;

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);
        }

        private static async Task AssertAccountNotFound(
            BadRequestObjectResult actual, AccountController sut, int accountId)
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var expectedMessage = "Account Not Found";
            actual.Value.Should().BeEquivalentTo(expectedMessage);

            sut.AccountIdValidator.Received().Validate(accountId);
            await sut.AccountReader.Received().GetAccountByIdAsync(accountId);
        }

        private static void SetupAccountNotFound(
            AccountController sut, ValidateResultModel validationResult)
        {
            validationResult.IsValid = true;

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).ReturnsNull();
        }

        private static void AssertInValidValidator(
            BadRequestObjectResult actual, AccountController sut,
            int accountId, ValidateResultModel validationResult)
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().BeEquivalentTo(validationResult.ErrorMessage);

            sut.AccountIdValidator.Received().Validate(accountId);
        }

        private static void SetupInvalidValidator(
            ValidateResultModel validationResult, AccountController sut)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessage = "error";

            sut.AccountIdValidator
                .Validate(Arg.Any<int>()).Returns(validationResult);
        }
    }
}
