﻿namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkTrade
{
    [JsonProperty("pair")]
    public string Symbol { get; set; }

    [JsonProperty("pairNormalized")]
    public string SymbolNormalized { get; set; }

    [JsonProperty("numerator")]
    public string Numerator { get; set; }

    [JsonProperty("denominator")]
    public string Denominator { get; set; }

    [JsonProperty("date"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    [JsonProperty("tid")]
    public string TradeId { get; set; } = "";

    [JsonProperty("price")]
    public decimal Price { get; set; }
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("side"), JsonConverter(typeof(OrderSideConverter))]
    public BtcTurkOrderSide Side { get; set; }

}
