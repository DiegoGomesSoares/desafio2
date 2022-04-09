namespace Domain.Models.Requests
{
    public class StatementRequest
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
