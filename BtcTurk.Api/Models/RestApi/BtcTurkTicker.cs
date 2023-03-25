namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkTicker
{
    [JsonProperty("pair")]
    public string Symbol { get; set; }

    [JsonProperty("pairNormalized")]
    public string SymbolNormalized { get; set; }

    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    [JsonProperty("ask")]
    public decimal Ask { get; set; }

    [JsonProperty("bid")]
    public decimal Bid { get; set; }

    [JsonProperty("open")]
    public decimal Open { get; set; }

    [JsonProperty("high")]
    public decimal High { get; set; }

    [JsonProperty("low")]
    public decimal Low { get; set; }

    [JsonProperty("last")]
    public decimal Close { get; set; }

    [JsonProperty("volume")]
    public decimal Volume { get; set; }

    [JsonProperty("average")]
    public decimal Average { get; set; }

    [JsonProperty("daily")]
    public decimal DailyChange { get; set; }

    [JsonProperty("dailyPercent")]
    public decimal DailyChangePercent { get; set; }

    [JsonProperty("denominatorSymbol")]
    public string DenominatorCurrency { get; set; }

    [JsonProperty("numeratorSymbol")]
    public string NumeratorCurrency { get; set; }

    [JsonProperty("order")]
    public int OrderNumber { get; set; }
}
