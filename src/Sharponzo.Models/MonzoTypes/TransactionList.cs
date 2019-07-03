using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sharponzo.Models.MonzoTypes
{
    public class TransactionList
    {
        [JsonProperty("transactions")]
        public IList<Transaction> Transactions { get; set; }
    }
}
