using Newtonsoft.Json;
using System.Globalization;

namespace Sharponzo.Models.Monzo
{
    public class Balance
    {
        [JsonProperty("balance")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("spend_today")]
        public int SpendToday { get; set; }

        public string FormattedBalance
        {
            get
            {
                var value = (double)Amount / 100;
                return value.ToString("C", CultureInfo.CurrentCulture); // TODO
            }
        }
    }
}
