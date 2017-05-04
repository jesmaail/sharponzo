using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sharponzo.MonzoTypes
{
    public class Merchant
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }
        
        [JsonProperty("atm")]
        public bool Atm { get; set; }
    }
}
