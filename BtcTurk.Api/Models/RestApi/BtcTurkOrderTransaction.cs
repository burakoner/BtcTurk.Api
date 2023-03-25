namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkOrderTransaction
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("preciseAmount")]
    public decimal PreciseAmount { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("numeratorSymbol")]
    public string Numerator { get; set; }

    [JsonProperty("denominatorSymbol")]
    public string Denominator { get; set; }

    [JsonProperty("fee")]
    public decimal Fee { get; set; }

    [JsonProperty("tax")]
    public decimal Tax { get; set; }

    [JsonProperty("orderType"), JsonConverter(typeof(OrderSideConverter))]
    public BtcTurkOrderSide Side { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("orderClientId")]
    public string ClientOrderId { get; set; }
}
