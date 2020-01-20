using Sharponzo.Models.Monzo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharponzo.Logic
{
    public class Filter
    {
        private IEnumerable<Transaction> _transactions;

        public Filter(IEnumerable<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public IEnumerable<Transaction> GetPaymentsByMerchant(string merchant)
        {
            return _transactions.Where(payment => string.Equals(payment.Merchant.Name, merchant, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public IEnumerable<Transaction> GetPaymentsByCategory(string category)
        {
            return _transactions
                .Where(payment => string.Equals(payment.Category, category, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public IEnumerable<Transaction> GetPaymentsByDateRange(DateTime start, DateTime end)
        {
            return _transactions.Where(payment => (payment.Date >= start) && (payment.Date <= end)).ToList();
        }

        public IEnumerable<Transaction> GetPaymentsByDate(DateTime date)
        {
            return _transactions.Where(payment => payment.Date.Date == date.Date).ToList();
        }

        public IEnumerable<Transaction> GetPaymentsByWeek(DateTime startOfWeek)
        {
            return GetPaymentsByDateRange(startOfWeek, startOfWeek.AddDays(6));
        }
    }
}
