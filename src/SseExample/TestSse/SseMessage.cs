using System.Text.Json.Serialization;

namespace TestSse
{
    public record SseMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = null!;
        [JsonPropertyName("message")]
        public object Message { get; init; } = null!;
    }
    public record SseClientId
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; init; } = null!;
    }
}
