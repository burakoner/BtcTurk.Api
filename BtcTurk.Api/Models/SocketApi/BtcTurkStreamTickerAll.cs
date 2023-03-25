namespace BtcTurk.Api.Models.SocketApi;

public class BtcTurkStreamTickerAll : BtcTurkStream
{
    [JsonProperty("items")]
    public List<BtcTurkStreamTickerRow> Items { get; set; } = new List<BtcTurkStreamTickerRow>();
}