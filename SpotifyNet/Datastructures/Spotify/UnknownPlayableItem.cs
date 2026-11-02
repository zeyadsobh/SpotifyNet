using System.Text.Json;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record UnknownPlayableItem : PlayableItem
{
    /// <summary>The item's <c>type</c> field, or <see langword="null"/> if it had none.</summary>
    public required string? Type { get; init; }

    public required JsonElement Json { get; init; }
}
