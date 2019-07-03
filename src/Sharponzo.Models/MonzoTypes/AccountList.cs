using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sharponzo.Models.MonzoTypes
{
    public class AccountList
    {
        [JsonProperty("accounts")]
        public IList<Account> Accounts { get; set; }
    }
}
