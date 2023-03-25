namespace BtcTurk.Api;

public class BtcTurkStreamClientOptions : StreamApiClientOptions
{
    public static BtcTurkStreamClientOptions Default { get; set; } = new();

    public BtcTurkStreamClientOptions() : base()
    {
        // Base Address
        this.BaseAddress = BtcTurkAddresses.Default.WebsocketAddress;
    }
}