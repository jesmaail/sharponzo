using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sharponzo.MonzoTypes
{
    public class Balance
    {
        [JsonProperty("balance")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currecy { get; set; }

        [JsonProperty("spend_today")]
        public int SpendToday { get; set; }
    }
}
