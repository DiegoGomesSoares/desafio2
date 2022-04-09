using Domain.Models.Requests;
using Domain.Operations;
using Domain.Repository.Reader;
using Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Payment.Attributes;
using System;
using System.Threading.Tasks;

namespace Payment.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        public IAccountReader AccountReader { get; }
        public IValidationHandler ValidationHandler { get; }
        public IAccountOperator AccountOperator { get; }

        public AccountTransactionController(
            IAccountReader accountReader,
            IValidationHandler validationHandler,
            IAccountOperator accountOperator)
        {
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
            ValidationHandler = validationHandler ?? throw new ArgumentNullException(nameof(validationHandler));
            AccountOperator = accountOperator ?? throw new ArgumentNullException(nameof(accountOperator));
        }

        [HttpPost]
        [ValidateViewModel]
        [Route("account/cashin")]
        [Route("account/cashout")]
        public async Task<IActionResult> Cashin([FromBody] OperationRequest model)
        {
            var account = await AccountReader.GetAccountByIdAsync(model.AccountId);

            var validationResult = ValidationHandler.Handle(account, model.Amount);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var response = await AccountOperator.ExecuteOperationAsync(account, model.Amount);

            return Ok(response);
        }
    }
}
