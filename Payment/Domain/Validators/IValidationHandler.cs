using Domain.Entities;
using Domain.Models;

namespace Domain.Validators
{
    public interface IValidationHandler
    {
        ValidateResultModel Handle(Conta account, decimal amount);
    }
}
