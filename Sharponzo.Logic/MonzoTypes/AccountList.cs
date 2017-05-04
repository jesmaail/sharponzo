using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sharponzo.MonzoTypes
{
    public class AccountList
    {
        [JsonProperty("accounts")]
        public IList<Account> Accounts { get; set; }
    }
}
