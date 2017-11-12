using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TransactionList = System.Collections.Generic.ICollection<Sharponzo.MonzoTypes.Transaction>;

namespace Sharponzo
{
    public class Filter
    {
        private TransactionList _transactions;

        public Filter(TransactionList transactions)
        {
            _transactions = transactions;
        }

        public TransactionList GetPaymentsByMerchant(string merchant)
        {
            return _transactions.Where(payment => string.Equals(payment.Merchant.Name, merchant, StringComparison.InvariantCultureIgnoreCase)).ToList();
        } 

        public TransactionList GetPaymentsByCategory(string category)
        {
            return _transactions.Where(payment => string.Equals(payment.Category, category, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public TransactionList GetPaymentsByDateRange(DateTime start, DateTime end)
        {
            return _transactions.Where(payment => (payment.Date >= start) && (payment.Date <= end)).ToList();
        }

        public TransactionList GetPaymentsByDate(DateTime date)
        {
            return _transactions.Where(payment => payment.Date.Date == date.Date).ToList();
        }

        public TransactionList GetPaymentsByWeek(DateTime startOfWeek)
        {
            return GetPaymentsByDateRange(startOfWeek, startOfWeek.AddDays(6));
        }
    }
}
