namespace BtcTurk.Api.Models.SocketApi;

public class BtcTurkStreamRequest(int type, string channel, string evt, bool join)
{
    [JsonProperty("type")]
    public int Type { get; set; } = type;

    [JsonProperty("channel")]
    public string Channel { get; set; } = channel;

    [JsonProperty("event")]
    public string Event { get; set; } = evt;

    [JsonProperty("join")]
    public bool Join { get; set; } = join;

    public object[] RequestObject()
    {
        return [Type, this];
    }

    public string RequestString()
    {
        return JsonConvert.SerializeObject(RequestObject());
    }
}

public class BtcTurkSocketAuthRequest(int type, string apikey, long timestamp, long nonce, string signature)
{
    [JsonProperty("type")]
    public int Type { get; set; } = type;

    [JsonProperty("publicKey")]
    public string PublicKey { get; set; } = apikey;
    
    [JsonProperty("timestamp")]
    public long Timestamp { get; set; } = timestamp;

    [JsonProperty("nonce")]
    public long Nonce { get; set; } = nonce;

    [JsonProperty("signature")]
    public string Signature { get; set; } = signature;

    public object[] RequestObject()
    {
        return [Type, this];
    }

    public string RequestString()
    {
        return JsonConvert.SerializeObject(RequestObject());
    }
}

public class BtcTurkSocketAuthResponse
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("ok")]
    public bool OK { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}

public class BtcTurkSocketLoginRequest(int type, int id, string token, string username)
{
    [JsonProperty("type")]
    public int Type { get; set; } = type;

    [JsonProperty("id")]
    public int Id { get; set; } = id;

    [JsonProperty("token")]
    public string Token { get; set; } = token;

    [JsonProperty("username")]
    public string Username { get; set; } = username;

    public object[] RequestObject()
    {
        return [Type, this];
    }

    public string RequestString()
    {
        return JsonConvert.SerializeObject(RequestObject());
    }
}

public class BtcTurkSocketLoginResponse
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("ok")]
    public bool OK { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
