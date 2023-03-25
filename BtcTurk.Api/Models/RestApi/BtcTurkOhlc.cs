namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkOhlc
{
    [JsonProperty("pair")]
    public string Symbol { get; set; }

    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    [JsonProperty("open")]
    public decimal Open { get; set; }

    [JsonProperty("high")]
    public decimal High { get; set; }

    [JsonProperty("low")]
    public decimal Low { get; set; }

    [JsonProperty("close")]
    public decimal Close { get; set; }

    [JsonProperty("volume")]
    public decimal Volume { get; set; }

    [JsonProperty("total")]
    public decimal Total { get; set; }

    [JsonProperty("average")]
    public decimal Average { get; set; }

    [JsonProperty("dailyChangeAmount")]
    public decimal DailyChangeAmount { get; set; }

    [JsonProperty("dailyChangePercentage")]
    public decimal DailyChangePercentage { get; set; }
}
