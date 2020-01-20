using Newtonsoft.Json;

namespace Sharponzo.Models.Monzo
{
    public class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; } 

        [JsonProperty("description")]
        public string Name { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }
    }
}
