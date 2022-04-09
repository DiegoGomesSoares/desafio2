using AutoFixture.Idioms;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.Filters;
using System.Net;
using System.Threading.Tasks;
using UnitTests.AutoData;
using Xunit;

namespace UnitTests.Payment.Filters
{
    public class ExceptionFilterTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ExceptionFilter).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task OnExceptionAsync_(
            ExceptionContext context,
            ExceptionFilter sut)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            await sut.OnExceptionAsync(context);

            context.HttpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
