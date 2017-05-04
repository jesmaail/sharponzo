using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sharponzo.MonzoTypes
{
    public class TransactionList
    {
        [JsonProperty("transactions")]
        public IList<Transaction> Transactions { get; set; }
    }
}
