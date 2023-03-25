namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkExchangeInfo
{
    [JsonProperty("timeZone")]
    public string TimeZone { get; set; }

    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }

    [JsonProperty("symbols")]
    public IEnumerable<BtcTurkSymbol> Symbols { get; set; }

    [JsonProperty("currencies")]
    public IEnumerable<BtcTurkCurrency> Currencies { get; set; }

    [JsonProperty("currencyOperationBlocks")]
    public IEnumerable<BtcTurkCurrencyOperationBlock> CurrencyOperationBlocks { get; set; }
}

public class BtcTurkCurrencyOperationBlock
{
    [JsonProperty("currencySymbol")]
    public string Symbol { get; set; }

    [JsonProperty("depositDisabled")]
    public bool DepositDisabled { get; set; }

    [JsonProperty("withdrawalDisabled")]
    public bool WithdrawalDisabled { get; set; }
}