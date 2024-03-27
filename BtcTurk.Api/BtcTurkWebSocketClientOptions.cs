namespace BtcTurk.Api;

public class BtcTurkWebSocketClientOptions : WebSocketApiClientOptions
{
    public static BtcTurkWebSocketClientOptions Default { get; set; } = new();

    public BtcTurkWebSocketClientOptions() : base()
    {
        // Base Address
        this.BaseAddress = BtcTurkAddresses.Default.WebsocketAddress;
    }
}