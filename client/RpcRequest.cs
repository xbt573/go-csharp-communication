using System.Text.Json.Serialization;

public class RpcRequest
{
    [JsonPropertyName("method")]
    public string   Method { get; init; }

    [JsonPropertyName("params")]
    public string[] Params { get; init; }

    [JsonPropertyName("id")]
    public int      Id     { get; init; }
}
