﻿namespace BtcTurk.Api.Models.SocketApi;

public class BtcTurkStreamTradeRow
{
    [JsonProperty("D"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    [JsonProperty("I")]
    public string TradeId { get; set; }

    [JsonProperty("A")]
    public decimal Amount { get; set; }

    [JsonProperty("P")]
    public decimal Price { get; set; }

    [JsonProperty("PS")]
    public string PairSymbol { get; set; }

    [JsonProperty("S")]
    public int S { get; set; }
}