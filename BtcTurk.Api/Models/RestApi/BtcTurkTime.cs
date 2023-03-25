namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkTime
{
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}
