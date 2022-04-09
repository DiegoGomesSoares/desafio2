using Domain.Enums;

namespace Domain.Models.Requests
{
    public class AccountRequest
    {
        public int PersonId { get; set; }
        public decimal? DailyCashOutLimit { get; set; }
        public AccountTypeEnum Type { get; set; }
    }
}
