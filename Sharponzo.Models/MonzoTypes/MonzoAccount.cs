using System.Collections.Generic;

namespace Sharponzo.Models.MonzoTypes
{
    public class MonzoAccount
    {
        public Balance Balance { get; set; }
        public IEnumerable<Transaction> Transactions{ get; set; }
        public IEnumerable<Transaction> Payments { get; set; }
        public IEnumerable<Transaction> Topups { get; set; }
        public IEnumerable<string> Merchants { get; set; }
    }
}
