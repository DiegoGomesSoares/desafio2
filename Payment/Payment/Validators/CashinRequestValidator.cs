using Domain.Models.Requests;
using FluentValidation;

namespace Payment.Validators
{
    public class CashinRequestValidator : AbstractValidator<CashinRequest>
    {
        public CashinRequestValidator()
        {
            RuleFor(x => x.AccountId)
               .GreaterThan(0)
               .NotNull();

            RuleFor(x => x.Amount)
                .NotNull();
        }
    }
}
