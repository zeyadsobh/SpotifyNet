using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>The cursors that find the next and previous page of a cursor-paged list.</summary>
public sealed record Cursors
{
    /// <summary>The cursor to use as key to find the next page of items.</summary>
    [JsonPropertyName("after")]
    public string? After { get; init; }

    /// <summary>The cursor to use as key to find the previous page of items.</summary>
    [JsonPropertyName("before")]
    public string? Before { get; init; }
}
