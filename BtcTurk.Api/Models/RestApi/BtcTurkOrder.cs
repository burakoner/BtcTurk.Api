namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkOrder
{
    [JsonProperty("id")]
    public long OrderId { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("leftAmount")]
    public decimal LeftAmount { get; set; }

    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    [JsonProperty("pairSymbol")]
    public string Symbol { get; set; }

    [JsonProperty("pairSymbolNormalized")]
    public string SymbolNormalized { get; set; }

    [JsonProperty("type"), JsonConverter(typeof(OrderSideConverter))]
    public BtcTurkOrderSide Side { get; set; }

    [JsonProperty("method"), JsonConverter(typeof(OrderMethodConverter))]
    public BtcTurkOrderMethod Method { get; set; }

    [JsonProperty("orderClientId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OrderTime { get; set; }

    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    [JsonProperty("status"), JsonConverter(typeof(OrderStatusConverter))]
    public BtcTurkOrderStatus Status { get; set; }
}
