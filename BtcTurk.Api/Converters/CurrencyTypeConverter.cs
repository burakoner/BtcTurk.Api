namespace BtcTurk.Api.Converters;

public class CurrencyTypeConverter : BaseConverter<BtcTurkCurrencyType>
{
    public CurrencyTypeConverter() : this(true) { }
    public CurrencyTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BtcTurkCurrencyType, string>> Mapping => new List<KeyValuePair<BtcTurkCurrencyType, string>>
    {
        new KeyValuePair<BtcTurkCurrencyType, string>(BtcTurkCurrencyType.Fiat, "FIAT"),
        new KeyValuePair<BtcTurkCurrencyType, string>(BtcTurkCurrencyType.Crypto, "CRYPTO"),
    };
}
