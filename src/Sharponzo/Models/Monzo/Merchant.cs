using Newtonsoft.Json;

namespace Sharponzo.Models.Monzo
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
