using System;
using NUnit.Framework;
using Sharponzo.MonzoTypes;
using System.Collections.Generic;

namespace Sharponzo.Test
{
    [TestFixture]
    [Category("Filter Tests")]
    public class FilterTests
    {
        Filter _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new Filter(_transactions);
        }

        [TestCase("Tesco", 2)]
        [TestCase("Amazon", 1)]
        [TestCase("Test", 0)]
        public void Filters_Payments_By_Merchant(string merchant, int expectedCount)
        {
            var actual = _filter.GetPaymentsByMerchant(merchant);
            
            Assert.AreEqual(expectedCount, actual.Count);
        }

        [TestCase("Test", 1)]
        [TestCase("Foo", 2)]
        [TestCase("None", 0)]
        public void Filters_Payments_By_Category(string category, int expectedCount)
        {
            var actual = _filter.GetPaymentsByCategory(category);

            Assert.AreEqual(expectedCount, actual.Count);
        }
        
        [TestCase("2017/01/02", "2017/01/05", 2)]
        [TestCase("2015/01/02", "2015/01/02", 0)]
        public void Filters_Payments_By_Date_Range(DateTime start, DateTime end, int expectedCount)
        {
            var actual = _filter.GetPaymentsByDateRange(start, end);

            Assert.AreEqual(expectedCount, actual.Count);
        }

        [TestCase("2017/01/02", 1)]
        [TestCase("2017/03/01", 2)]
        [TestCase("2015/01/02", 0)]
        public void Filters_Payments_By_Date(DateTime date, int expectedCount)
        {
            var actual = _filter.GetPaymentsByDate(date);

            Assert.AreEqual(expectedCount, actual.Count);
        }

        [TestCase("2017/01/02", 2)]
        [TestCase("2015/01/02", 0)]
        public void Filters_Payments_By_Week(DateTime startOfWeek, int expectedCount)
        {
            var actual = _filter.GetPaymentsByWeek(startOfWeek);

            Assert.AreEqual(expectedCount, actual.Count);
        }

        List<Transaction> _transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = "00",
                Date = new DateTime(2017,01,02),
                Description = "TestTransactionZero",
                Amount = 123,
                Currency = "GBP",
                Merchant = new Merchant
                {
                    Name = "Tesco",
                    Online = false,
                    Atm = false
                },
                Notes = "foo, bar",
                AccountBalance = 1234,
                Category = "Test",
                IsLoad = false
            },
            new Transaction
            {
                Id = "01",
                Date = new DateTime(2017,03,01),
                Description = "TestTransactionOne",
                Amount = 123,
                Currency = "GBP",
                Merchant = new Merchant
                {
                    Name = "Tesco",
                    Online = false,
                    Atm = false
                },
                Notes = "foo, bar",
                AccountBalance = 1234,
                Category = "Foo",
                IsLoad = false
            },
            new Transaction
            {
                Id = "02",
                Date = new DateTime(2017,01,03),
                Description = "TestTransactionTwo",
                Amount = 123,
                Currency = "GBP",
                Merchant = new Merchant
                {
                    Name = "Amazon",
                    Online = true,
                    Atm = false
                },
                Notes = "foo, bar",
                AccountBalance = 1234,
                Category = "Foo",
                IsLoad = false
            },
            new Transaction
            {
                Id = "03",
                Date = new DateTime(2017,03,01),
                Description = "TestTransactionThree",
                Amount = 123,
                Currency = "GBP",
                Merchant = new Merchant
                {
                    Name = "eBay",
                    Online = true,
                    Atm = false
                },
                Notes = "foo, bar",
                AccountBalance = 1234,
                Category = "Bar",
                IsLoad = false
            }
        };
    }
}
