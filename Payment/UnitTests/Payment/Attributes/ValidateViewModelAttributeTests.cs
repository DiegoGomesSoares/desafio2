using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Attributes
{
    public class ValidateViewModelAttributeTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_WhenModelStateIsValid_ShouldNotReturnBadRequest(
           ValidateViewModelAttribute sut,
           ActionExecutingContext context)
        {
            sut.OnActionExecuting(context);

            context.Result.Should().NotBeOfType<BadRequestObjectResult>();
        }

        [Theory, AutoNSubstituteData]
        public void Sut_WhenModelStateIsInValid_ShouldReturnBadRequestWithCorrectErrorMessage(
           ValidateViewModelAttribute sut,
           ActionExecutingContext context)
        {
            context.ModelState.AddModelError("Error", "message");

            sut.OnActionExecuting(context);

            context.Result.Should().BeOfType<BadRequestObjectResult>();
            var result = context.Result as BadRequestObjectResult;
            var errors = result.Value as List<ErrorModel>;
            errors.First().Field.Should().Be("Error");
            errors.First().Message.Should().Be("message");
        }
    }
}
