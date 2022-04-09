using Domain.Entities;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Domain.Operations;
using Domain.Repository.Reader;
using Domain.Repository.Writer;
using Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Payment.Attributes;
using System;
using System.Threading.Tasks;

namespace Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountCreateValidator AccountCreateValidator { get; }
        public IAccountIdValidator AccountIdValidator { get; }
        public IAccountWriter AccountWriter { get; }
        public IAccountReader AccountReader { get; }
        public IStatementOperator StatementOperator { get; }

        private static readonly string _accountNotFoundErrorMessage = "Account Not Found";
        private static readonly string _accountIsBlockErrorMessage = "Account Is Already Blocked";

        public AccountController(
            IAccountCreateValidator accountCreateValidator,
            IAccountIdValidator accountIdValidator,
            IAccountWriter accountWriter,
            IAccountReader accountReader,
            IStatementOperator statementOperator)
        {
            AccountCreateValidator = accountCreateValidator ?? throw new ArgumentNullException(nameof(accountCreateValidator));
            AccountIdValidator = accountIdValidator ?? throw new ArgumentNullException(nameof(accountIdValidator));
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));
            StatementOperator = statementOperator ?? throw new ArgumentNullException(nameof(statementOperator));
        }

        [HttpPost]
        [ValidateViewModel]
        [Route("/create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest model)
        {
            var validationResult = await AccountCreateValidator.ValidateAsync(model.PersonId, model.Type);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var account = GetAccountToCreate(model);

            account.IdConta = await AccountWriter.CreateAsync(account);

            return Ok(GetAccounteResponse(account));
        }

        [HttpGet]
        [Route("/balance/{accountId}")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var validationResult = AccountIdValidator.Validate(accountId);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var account = await AccountReader.GetAccountByIdAsync(accountId);
            if (account == null)
                return BadRequest(_accountNotFoundErrorMessage);

            return Ok(GetBalanceResponse(account.Saldo));
        }

        [HttpPut]
        [Route("/block/{accountId}")]
        public async Task<IActionResult> Block(int accountId)
        {
            var validationResult = AccountIdValidator.Validate(accountId);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var account = await AccountReader.GetAccountByIdAsync(accountId);
            if (account == null)
                return BadRequest(_accountNotFoundErrorMessage);

            if (account.FlagAtivo == false)
                return BadRequest(_accountIsBlockErrorMessage);

            await AccountWriter.BlockAccount(accountId);

            return Ok();
        }

        [HttpGet]
        [Route("/statement/{accountId}")]
        public async Task<IActionResult> GetStatement(
            [FromRoute] int accountId,
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            [FromQuery] int size = 10,
            [FromQuery] int page = 1)
        {
            var validationResult = AccountIdValidator.Validate(accountId);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var account = await AccountReader.GetAccountByIdAsync(accountId);
            if (account == null)
                return BadRequest(_accountNotFoundErrorMessage);

            if (account.FlagAtivo == false)
                return BadRequest(_accountIsBlockErrorMessage);

            var statementRequest = new StatementRequest
            {
                AccountId = accountId,
                Balance = account.Saldo,
                StartDate = startDate,
                EndDate = endDate,
                Size = size,
                Page = page
            };

            var response = await StatementOperator.GetStatementAsync(statementRequest);

            return Ok(response);
        }

        private static GetBalanceResponse GetBalanceResponse(decimal saldo)
        {
            return new GetBalanceResponse { Saldo = saldo };
        }

        private static AccountResponse GetAccounteResponse(Conta account)
        {
            return new AccountResponse
            {
                IdConta = account.IdConta,
                Saldo = account.Saldo,
                DataCriacao = account.DataCriacao
            };
        }

        private static Conta GetAccountToCreate(AccountRequest model)
        {
            return new Conta
            {
                FlagAtivo = true,
                IdPessoa = model.PersonId,
                LimiteSaqueDiario = model.DailyCashOutLimit,
                TipoConta = (short)model.Type,
                Saldo = 0,
                DataCriacao = DateTime.Now
            };
        }
    }
}
