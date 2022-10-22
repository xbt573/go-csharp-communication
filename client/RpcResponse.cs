using System.Text.Json.Serialization;

public class RpcResponse
{
    [JsonPropertyName("result")]
    public string Result { get; init; }

    [JsonPropertyName("error")]
    public string Error  { get; init; }

    [JsonPropertyName("id")]
    public int    Id     { get; init; }
}
