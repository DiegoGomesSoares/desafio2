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

        public IAccountTransactionValidator AccountTransactionValidator { get; }
        public IAccountOperator AccountOperator { get; }

        public AccountTransactionController(
            IAccountReader accountReader,
            IAccountTransactionValidator accountTransactionValidator,
            IAccountOperator accountOperator)
        {
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
            AccountTransactionValidator = accountTransactionValidator ?? throw new ArgumentNullException(nameof(accountTransactionValidator));
            AccountOperator = accountOperator ?? throw new ArgumentNullException(nameof(accountOperator));
        }

        [HttpPost]
        [ValidateViewModel]
        [Route("account/cashin")]
        public async Task<IActionResult> Create([FromBody] CashinRequest model)
        {
            var account = await AccountReader.GetAccountByIdAsync(model.AccountId);

            var validationResult = AccountTransactionValidator.ValidateCashinOperation(account);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var response = await AccountOperator.ExecuteOperationAsync(account, model.Amount);

            return Ok(response);
        }
    }
}
