namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkCurrency
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("minWithdrawal")]
    public decimal MinWithdrawal { get; set; }

    [JsonProperty("minDeposit")]
    public decimal MinDeposit { get; set; }

    [JsonProperty("precision")]
    public int Precision { get; set; }

    [JsonProperty("currencyType"), JsonConverter(typeof(CurrencyTypeConverter))]
    public BtcTurkCurrencyType Type{ get; set; }

    public BtcTurkCurrencyAddress Address { get; set; }
    public BtcTurkCurrencyTag Tag { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public bool IsNew { get; set; }
    public bool IsAddressRenewable { get; set; }
    public bool GetAutoAddressDisabled { get; set; }
    public bool IsPartialWithdrawalEnabled { get; set; }
}

public class BtcTurkCurrencyAddress
{
    [JsonProperty("minLen")]
    public string MinLength { get; set; }

    [JsonProperty("maxLen")]
    public string MaxLength { get; set; }
}

public class BtcTurkCurrencyTag
{
    public bool Enable { get; set; }
    public string Name { get; set; }

    [JsonProperty("minLen")]
    public string MinLength { get; set; }

    [JsonProperty("maxLen")]
    public string MaxLength { get; set; }
}
