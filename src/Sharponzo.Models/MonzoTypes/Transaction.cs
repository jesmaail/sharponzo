using System;
using Newtonsoft.Json;

namespace Sharponzo.Models.MonzoTypes
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public DateTime Date { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("account_balance")]
        public int AccountBalance { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("is_load")]
        public bool IsLoad { get; set; }
    }
}
