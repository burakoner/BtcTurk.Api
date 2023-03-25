namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkPlacedOrder
{
    [JsonProperty("id")]
    public long OrderId { get; set; }

    [JsonProperty("datetime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Datetime { get; set; }

    [JsonProperty("type"), JsonConverter(typeof(OrderSideConverter))]
    public BtcTurkOrderSide Side { get; set; }

    [JsonProperty("method"), JsonConverter(typeof(OrderMethodConverter))]
    public BtcTurkOrderMethod Method { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    [JsonProperty("quantity")]
    public decimal? Quantity { get; set; }

    [JsonProperty("pairSymbol")]
    public string Symbol { get; set; }

    [JsonProperty("pairSymbolNormalized")]
    public string SymbolNormalized { get; set; }

    [JsonProperty("newOrderClientId")]
    public string ClientOrderId { get; set; }
}
