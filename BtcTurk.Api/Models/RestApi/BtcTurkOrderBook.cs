namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkOrderBook
{
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    [JsonProperty("asks")]
    public IEnumerable<BtcTurkOrderBookEntry> Asks { get; set; }

    [JsonProperty("bids")]
    public IEnumerable<BtcTurkOrderBookEntry> Bids { get; set; }
}

[JsonConverter(typeof(ArrayConverter))]
public class BtcTurkOrderBookEntry
{
    /// <summary>
    /// The price for this entry
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity for this entry
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }

}
