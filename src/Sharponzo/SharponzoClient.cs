using Sharponzo.Models.Monzo;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using System.Linq;

namespace Sharponzo
{
    public class SharponzoClient
    {
        private RestClient _client;

        public SharponzoClient(string accessToken)
        {
            _client = new RestClient("https://api.monzo.com");
            _client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
        }
        
        public IList<Account> GetAccountList()
        {
            var request = new RestRequest("/accounts");
            var response = _client.Execute(request);

            return JsonConvert.DeserializeObject<AccountList>(response.Content).Accounts;
        }

        public MonzoAccount GetAccount(string accountIndex)
        {
            var transactions = GetTransactions(accountIndex);
            var payments = GetPayments(transactions);
            var topups = GetTopups(transactions);
            var merchants = GetMerchantNames(payments);

            return new MonzoAccount
            {
                Balance = GetAccountBalance(accountIndex),
                Transactions = transactions,
                Payments = payments,
                Topups = topups,
                Merchants = merchants
            };
        }

        private Balance GetAccountBalance(string accountIndex)
        {
            var request = new RestRequest($"/balance?account_id={accountIndex}");
            var response = _client.Execute(request);

            return JsonConvert.DeserializeObject<Balance>(response.Content);
        }

        private IEnumerable<Transaction> GetTransactions(string accountIndex)
        {
            var request = new RestRequest($"/transactions?expand[]=merchant&account_id={accountIndex}");
            var response = _client.Execute(request);

            return JsonConvert.DeserializeObject<TransactionList>(response.Content).Transactions;
        }
        
        private IEnumerable<Transaction> GetPayments(IEnumerable<Transaction> transactions)
        {
            return transactions
                .Where(t => !t.IsLoad)
                .ToList();
        }
        
        private IEnumerable<Transaction> GetTopups(IEnumerable<Transaction> transactions)
        {
            return transactions
                .Where(t => t.IsLoad)
                .ToList();
        }

        private IEnumerable<string> GetMerchantNames(IEnumerable<Transaction> payments)
        {
            return payments.Select(p => p.Merchant.Name)
                .Distinct()
                .ToList();
        }
    }
}
