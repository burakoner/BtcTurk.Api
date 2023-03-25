namespace BtcTurk.Api.Authentication;

public class BtcTurkAuthenticationProvider : AuthenticationProvider
{
    public BtcTurkAuthenticationProvider(ApiCredentials credentials) : base(credentials)
    {
        if (credentials == null || credentials.Secret == null)
            throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");
    }

    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, bool signed, ArraySerialization serialization, SortedDictionary<string, object> query, SortedDictionary<string, object> body, string bodyContent, SortedDictionary<string, string> headers)
    {
        if (!signed)
            return;

        uri = uri.SetParameters(query, serialization);
        if (uri.AbsoluteUri.Contains("/v1/"))
        {
            var sign = string.Empty;
            var apiKey = Credentials.Key.GetString();
            var apiSecret = Credentials.Secret.GetString();
            var nonce = DateTime.UtcNow.ToUnixTimeMilliseconds();
            string message = apiKey + nonce;
            using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(apiSecret)))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                sign = Convert.ToBase64String(signatureBytes);
            }

            headers.Add("X-PCK", Credentials.Key!.GetString());
            headers.Add("X-Stamp", nonce.ToString());
            headers.Add("X-Signature", sign);
        }
    }
    
    public override void AuthenticateSocketApi()
    {
        throw new NotImplementedException();
    }

    public override void AuthenticateStreamApi()
    {
        throw new NotImplementedException();
    }
}