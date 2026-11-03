using System.Text.Json;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A playable item that's neither a track nor an episode, kept as its raw JSON.</summary>
public sealed record UnknownPlayableItem : PlayableItem
{
    /// <summary>The item's <c>type</c> field, or <see langword="null"/> if it had none.</summary>
    public required string? Type { get; init; }

    /// <summary>The item's JSON, as received.</summary>
    public required JsonElement Json { get; init; }
}
