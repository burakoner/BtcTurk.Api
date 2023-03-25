namespace BtcTurk.Api;

public class BtcTurkAddresses
{
    public string ApiAddress { get; set; }
    public string GraphApiAddress { get; set; }
    public string WebsocketAddress { get; set; }

    public static BtcTurkAddresses Default = new BtcTurkAddresses
    {
        ApiAddress = "https://api.btcturk.com/api",
        GraphApiAddress = "https://graph-api.btcturk.com",
        WebsocketAddress = "wss://ws-feed-pro.btcturk.com"
        // WebsocketAddress = "wss://ws-feed-sandbox.btctrader.com";
    };
}