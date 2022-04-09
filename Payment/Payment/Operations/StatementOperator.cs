using Domain.Models.Requests;
using Domain.Operations;
using Domain.Repository.Reader;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Payment.Operations
{
    public class StatementOperator : IStatementOperator
    {
        public ITransactionReader TransactionReader { get; }

        public StatementOperator(
            ITransactionReader transactionReader)
        {
            TransactionReader = transactionReader ?? throw new ArgumentNullException(nameof(transactionReader));
        }

        public async Task<StatementResponse> GetStatementAsync(StatementRequest model)
        {
            var startDate = GetFormatedStartDate(model.StartDate);
            var endDate = GetFormatedEndDate(model.EndDate);

            var countTask = TransactionReader.GetTotalCountAsync(model.AccountId, startDate, endDate);
            var transactionTask =
                        TransactionReader
                        .GetAllPaginatedAsync(model.AccountId, startDate, endDate, model.Page, model.Size);

            await Task.WhenAll(countTask, transactionTask);

            var count = await countTask;
            var transactions = await transactionTask;

            var totalPages = (int)Math.Ceiling((double)count / model.Size);

            var response = StatementResponseHelpers.MapTransactions(transactions);
            response.IdConta = model.AccountId;
            response.Saldo = model.Balance;
            response.TotalPaginas = totalPages;
            response.PaginaIndex = model.Page;
            response.TotalTransacao = count;

            return response;
        }

        private static string GetFormatedEndDate(string endDate)
        {
            if (string.IsNullOrWhiteSpace(endDate)
                || DateTime.TryParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateConverted) == false)
                return DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");

            return endDateConverted.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd h:mm:ss tt");
        }

        private static string GetFormatedStartDate(string startDate)
        {
            if (string.IsNullOrWhiteSpace(startDate)
                || DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime starDateConverted) == false)
                return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd h:mm:ss tt");

            return starDateConverted.ToString("yyyy-MM-dd h:mm:ss tt");
        }
    }
}
