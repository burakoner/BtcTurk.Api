namespace BtcTurk.Api.Converters;

public class OrderMethodConverter : BaseConverter<BtcTurkOrderMethod>
{
    public OrderMethodConverter() : this(true) { }
    public OrderMethodConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BtcTurkOrderMethod, string>> Mapping => new List<KeyValuePair<BtcTurkOrderMethod, string>>
    {
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.Limit, "limit"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.Market, "market"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.StopLimit, "stoplimit"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.StopMarket, "stopmarket"),
    };
}

public class SymbolMethodConverter : BaseConverter<BtcTurkOrderMethod>
{
    public SymbolMethodConverter() : this(true) { }
    public SymbolMethodConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BtcTurkOrderMethod, string>> Mapping => new List<KeyValuePair<BtcTurkOrderMethod, string>>
    {
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.Limit, "LIMIT"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.Market, "MARKET"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.StopLimit, "STOP_LIMIT"),
        new KeyValuePair<BtcTurkOrderMethod, string>(BtcTurkOrderMethod.StopMarket, "STOP_MARKET"),
    };
}
