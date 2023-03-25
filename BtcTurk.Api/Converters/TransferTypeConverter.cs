namespace BtcTurk.Api.Converters;

public class TransferTypeConverter : BaseConverter<BtcTurkTransferType>
{
    public TransferTypeConverter() : this(true) { }
    public TransferTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BtcTurkTransferType, string>> Mapping => new List<KeyValuePair<BtcTurkTransferType, string>>
    {
        new KeyValuePair<BtcTurkTransferType, string>(BtcTurkTransferType.Deposit, "deposit"),
        new KeyValuePair<BtcTurkTransferType, string>(BtcTurkTransferType.Withdrawal, "withdrawal"),
    };
}
