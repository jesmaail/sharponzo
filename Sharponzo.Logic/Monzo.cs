using Sharponzo.MonzoTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sharponzo
{
    public class Monzo
    {
        private string _accountId;
        private IList<Account> _accounts;
        private IList<Transaction> _transactions;
        private Balance _balance;

        public Monzo(ApiLayer monzoApi)
        {
            _accounts = monzoApi.GetAccounts().Result;
            _accountId = _accounts[0].Id;
            _transactions = monzoApi.GetTransactions(_accountId).Result;
            _balance = monzoApi.GetBalance(_accountId).Result;
        }

        public ICollection<Transaction> GetAllPayments()
        {
            return _transactions.Where(transaction => !transaction.IsLoad).ToList();
        }

        public IEnumerable<Transaction> GetAllTopups()
        {
            return _transactions.Where(transaction => transaction.IsLoad).ToList();
        }

        public string GetCurrentBalance()
        {
            var value = (double)_balance.Amount / 100;
            return value.ToString("C", CultureInfo.CurrentCulture);
        }

        public string GetAccountHolder()
        {
            return _accounts[0].Name;
        }

        public IEnumerable<string> GetMerchantNames()
        {
            var result = new List<string>();
            var merchants = GetAllPayments().Select(payment => payment.Merchant.Name).ToList();
            result.AddRange(merchants.Distinct());
            return result;
        }

        public IEnumerable<string> GetCategories()
        {
            var result = new List<string>();
            var categories = GetAllPayments().Select(payment => payment.Category).ToList();
            result.AddRange(categories.Distinct());
            return result;
        }
    }
}
