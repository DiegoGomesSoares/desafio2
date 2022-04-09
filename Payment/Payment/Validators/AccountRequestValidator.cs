using Domain.Models.Requests;
using FluentValidation;

namespace Payment.Validators
{
    public class AccountRequestValidator : AbstractValidator<AccountRequest>
    {
        public AccountRequestValidator()
        {
            RuleFor(x => x.PersonId)
               .GreaterThan(0)
               .NotNull();

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.DailyCashOutLimit)
                .GreaterThan(0)
                .When(x => x.DailyCashOutLimit != null);

        }
    }
}
