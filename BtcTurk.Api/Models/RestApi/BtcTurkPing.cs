namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkPing
{
    [JsonProperty("pong")]
    public bool Pong { get; set; }
}
