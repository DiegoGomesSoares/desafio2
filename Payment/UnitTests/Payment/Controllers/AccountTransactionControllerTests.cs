using AutoFixture.Idioms;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Payment.Controllers;
using System.Net;
using System.Threading.Tasks;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Controllers
{
    public class AccountTransactionControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AccountTransactionController).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task Operation_WhenIsNotValid_ShouldReturnBadRequestWithErrorMessage(
            OperationRequest model,
            Conta account,
            ValidateResultModel validationResult,
            AccountTransactionController sut)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessage = "error";

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);
            sut.ValidationHandler
                .Handle(Arg.Any<Conta>(), Arg.Any<decimal>())
                .Returns(validationResult);

            var actual = await sut.Operation(model) as BadRequestObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().BeEquivalentTo(validationResult.ErrorMessage);

            await sut.AccountReader.Received().GetAccountByIdAsync(model.AccountId);
            sut.ValidationHandler.Received().Handle(account, model.Amount);
            await sut.AccountOperator.DidNotReceive().ExecuteOperationAsync(Arg.Any<Conta>(), Arg.Any<decimal>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Operation_WhenExecuteCorrectly_ShouldReturnHttpStatusCodeOkWithResponse(
            OperationRequest model,
            Conta account,
            ValidateResultModel validationResult,
            OperationResponse operationResponse,
            AccountTransactionController sut)
        {
            validationResult.IsValid = true;

            sut.AccountReader.GetAccountByIdAsync(Arg.Any<int>()).Returns(account);
            sut.ValidationHandler
                .Handle(Arg.Any<Conta>(), Arg.Any<decimal>())
                .Returns(validationResult);
            sut.AccountOperator.ExecuteOperationAsync(Arg.Any<Conta>(), Arg.Any<decimal>())
                .Returns(operationResponse);

            var actual = await sut.Operation(model) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(operationResponse);

            await sut.AccountReader.Received().GetAccountByIdAsync(model.AccountId);
            sut.ValidationHandler.Received().Handle(account, model.Amount);
            await sut.AccountOperator.Received().ExecuteOperationAsync(account, model.Amount);
        }
    }
}
