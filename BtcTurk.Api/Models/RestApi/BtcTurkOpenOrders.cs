namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkOpenOrders
{
    [JsonProperty("asks")]
    public IEnumerable<BtcTurkOrder> Asks { get; set; }

    [JsonProperty("bids")]
    public IEnumerable<BtcTurkOrder> Bids { get; set; }
}
