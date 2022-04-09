using Domain.Models.Requests;
using FluentValidation;

namespace Payment.Validators
{
    public class OperationRequestValidator : AbstractValidator<OperationRequest>
    {
        public OperationRequestValidator()
        {
            RuleFor(x => x.AccountId)
               .GreaterThan(0)
               .NotNull();

            RuleFor(x => x.Amount)
                .NotEqual(0)
                .NotNull();
        }
    }
}
