namespace BtcTurk.Api.Models.SocketApi;

[JsonConverter(typeof(WSObjectConverter))]
public class BtcTurkStreamResponse
{
    public int Model { get; set; }
    public string Data { get; set; } = "";
}

public class WSObjectConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var foo = value as BtcTurkStreamResponse;
        var obj = new object[] { foo.Model, foo.Data };
        serializer.Serialize(writer, obj);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var arr = ReadArrayObject(reader, serializer);
        return new BtcTurkStreamResponse
        {
            Model = (int)arr[0],
            Data = arr[1].ToString(),
        };
    }

    private JArray ReadArrayObject(JsonReader reader, JsonSerializer serializer)
    {
        var arr = serializer.Deserialize<JToken>(reader) as JArray;
        if (arr == null || arr.Count != 2)
            throw new JsonSerializationException("Expected array of length 2");
        return arr;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BtcTurkStreamResponse);
    }
}
