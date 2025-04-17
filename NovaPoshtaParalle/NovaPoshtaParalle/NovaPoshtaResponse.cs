using System.Text.Json.Serialization;

namespace NovaPoshtaParalle;

public class NovaPoshtaResponse<T>
{
    public T[] Data { get; set; }

    [JsonPropertyName("info")]
    public NovaPoshtaInfo Info { get; set; }
}

public class NovaPoshtaInfo
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}
