using Domain.Entities;
using Domain.Models.Requests;
using Domain.Models.Responses;
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
        public IGetBalanceValidator GetBalanceValidator { get; }
        public IAccountWriter AccountWriter { get; }
        public IAccountReader AccountReader { get; }

        private static readonly string _accountNotFoundErrorMessage = "Account Not Found";

        public AccountController(
            IAccountCreateValidator accountCreateValidator,
            IGetBalanceValidator getBalanceValidator,
            IAccountWriter accountWriter,
            IAccountReader accountReader)
        {
            AccountCreateValidator = accountCreateValidator ?? throw new ArgumentNullException(nameof(accountCreateValidator));
            GetBalanceValidator = getBalanceValidator ?? throw new ArgumentNullException(nameof(getBalanceValidator));
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
            AccountReader = accountReader ?? throw new ArgumentNullException(nameof(accountReader));            
        }

        [HttpPost]
        [ValidateViewModel]
        [Route("/create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest model)
        {
            var validationResult = await AccountCreateValidator.ValidateAsync(model.PersonId, model.Type);

            if (validationResult.IsValid == false )
                return BadRequest(validationResult.ErrorMessage);

            var account = GetAccountToCreate(model);

            account.IdConta = await AccountWriter.CreateAsync(account);

            return Ok(GetAccounteResponse(account));
        }

        [HttpGet]        
        [Route("/balance/{accountId}")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var validationResult = GetBalanceValidator.ValidateAsync(accountId);

            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ErrorMessage);

            var account = await AccountReader.GetAccountByIdAsync(accountId);
            if (account == null)
                return BadRequest(_accountNotFoundErrorMessage);

            return Ok(GetBalanceResponse(account.Saldo));

        }

        private GetBalanceResponse GetBalanceResponse(decimal saldo)
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

        private Conta GetAccountToCreate(AccountRequest model)
        {
            return new Conta
            {
                FlagAtivo = model.IsEnabled,
                IdPessoa = model.PersonId,
                LimiteSaqueDiario = model.DailyCashOutLimit,
                TipoConta = (short)model.Type,
                Saldo = 0,
                DataCriacao = DateTime.Now
            };
        }
    }
}
