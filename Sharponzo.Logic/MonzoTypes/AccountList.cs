using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sharponzo.MonzoTypes
{
    public class AccountList
    {
        [JsonProperty("accounts")]
        public IList<Account> Accounts { get; set; }
    }
}
