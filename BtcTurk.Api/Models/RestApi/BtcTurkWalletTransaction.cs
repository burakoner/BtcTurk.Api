namespace BtcTurk.Api.Models.RestApi;

public class BtcTurkWalletTransaction
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("preciseAmount")]
    public decimal PreciseAmount { get; set; }

    [JsonProperty("fee")]
    public decimal Fee { get; set; }

    [JsonProperty("tax")]
    public decimal Tax { get; set; }

    [JsonProperty("balanceType"), JsonConverter(typeof(TransferTypeConverter))]
    public BtcTurkTransferType TransactionType { get; set; }

    [JsonProperty("currencySymbol")]
    public string Currency { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("txHash")]
    public string TransactionHash { get; set; }

    [JsonProperty("confirmationCount")]
    public int? ConfirmationCount { get; set; }

    [JsonProperty("isConfirmed")]
    public bool IsConfirmed { get; set; }
}
