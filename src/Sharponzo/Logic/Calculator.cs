using Sharponzo.Models.Monzo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharponzo.Logic
{
    public class Calculator
    {
        private IEnumerable<Transaction> _transactions;
        private Filter _filter;

        public Calculator(IEnumerable<Transaction> transactions)
        {
            _transactions = transactions;
            _filter = new Filter(transactions);
        }

        public double GetSpendForDay(DateTime day)
        {
            var transactions = _filter.GetPaymentsByDate(day);
            var amount = GetPaymentsAmount(transactions);

            return amount;
        }

        public double GetPaymentsAmount(IEnumerable<Transaction> transactions)
        {
            double amount = transactions.Sum(t => t.Amount);
            amount = Math.Abs(amount);
            amount = amount / 100;

            return amount;
            //return (Math.Abs(transactions.Sum(t => t.Amount)) / 100);
        }

        public DateTime HighestSpendingDayInWeek(DateTime startOfWeek)
        {
            var max = 0.0;
            var dayIndex = 0;

            for (var i = 0; i < 7; i++)
            {
                var spend = GetSpendForDay(startOfWeek.AddDays(i));
                if (!(spend > max)) continue;
                max = spend;
                dayIndex = i;
            }

            return startOfWeek.AddDays(dayIndex);
        }
    }
}
