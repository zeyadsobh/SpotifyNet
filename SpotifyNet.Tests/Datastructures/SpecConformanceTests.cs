using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Chapters;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Search;
using SpotifyNet.Datastructures.Spotify.Shows;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;
using YamlDotNet.Serialization;
using Node = System.Collections.Generic.Dictionary<object, object>;
using Spotify = SpotifyNet.Datastructures.Spotify;

namespace SpotifyNet.Tests.Datastructures;

/// <summary>
/// Checks each model against open-api-schema.yaml: no deprecated fields, no missing or extra
/// fields, <c>required</c> only where the spec requires it, nullable unless required and
/// non-null (or deliberately relaxed), and matching types.
/// </summary>
public partial class SpecConformanceTests
{
    private static readonly Dictionary<string, Type> Models = new()
    {
        ["AlbumObject"] = typeof(Album),
        ["SimplifiedAlbumObject"] = typeof(SimplifiedAlbum),
        ["ArtistDiscographyAlbumObject"] = typeof(SimplifiedAlbum),
        ["SavedAlbumObject"] = typeof(SavedAlbum),
        ["ArtistObject"] = typeof(Artist),
        ["SimplifiedArtistObject"] = typeof(SimplifiedArtist),
        ["AudiobookObject"] = typeof(Audiobook),
        ["SimplifiedAudiobookObject"] = typeof(SimplifiedAudiobook),
        ["AuthorObject"] = typeof(Author),
        ["NarratorObject"] = typeof(Narrator),
        ["ChapterObject"] = typeof(Chapter),
        ["SimplifiedChapterObject"] = typeof(SimplifiedChapter),
        ["EpisodeObject"] = typeof(Episode),
        ["SimplifiedEpisodeObject"] = typeof(SimplifiedEpisode),
        ["SavedEpisodeObject"] = typeof(SavedEpisode),
        ["ShowObject"] = typeof(Show),
        ["SimplifiedShowObject"] = typeof(SimplifiedShow),
        ["SavedShowObject"] = typeof(SavedShow),
        ["TrackObject"] = typeof(Track),
        ["SimplifiedTrackObject"] = typeof(SimplifiedTrack),
        ["SavedTrackObject"] = typeof(SavedTrack),
        ["PlaylistObject"] = typeof(Playlist),
        ["SimplifiedPlaylistObject"] = typeof(SimplifiedPlaylist),
        ["PlaylistTrackObject"] = typeof(PlaylistTrack),
        ["PlaylistTracksRefObject"] = typeof(SimplifiedPlaylistTracks),
        ["PlaylistUserObject"] = typeof(PlaylistUser),
        ["PlaylistOwnerObject"] = typeof(PlaylistOwner),
        ["PlaylistSnapshotId"] = typeof(PlaylistSnapshot),
        ["PrivateUserObject"] = typeof(User),
        ["PlayHistoryObject"] = typeof(PlayHistory),
        ["DeviceObject"] = typeof(Device),
        ["ManyDevices"] = typeof(AvailableDevices),
        ["CurrentlyPlayingContextObject"] = typeof(CurrentlyPlayingContext),
        ["QueueObject"] = typeof(PlaybackQueue),
        ["DisallowsObject"] = typeof(Disallows),
        ["ErrorObject"] = typeof(Error),
        ["ContextObject"] = typeof(Spotify.Context),
        ["CopyrightObject"] = typeof(Copyright),
        ["ExternalIdObject"] = typeof(ExternalIds),
        ["ExternalUrlObject"] = typeof(ExternalUrls),
        ["ImageObject"] = typeof(Image),
        ["ResumePointObject"] = typeof(ResumePoint),
        ["AlbumRestrictionObject"] = typeof(Restrictions),
        ["ChapterRestrictionObject"] = typeof(Restrictions),
        ["EpisodeRestrictionObject"] = typeof(Restrictions),
        ["TrackRestrictionObject"] = typeof(Restrictions),
        ["CursorObject"] = typeof(Cursors),
        ["CursorPagingPlayHistoryObject"] = typeof(CursorPaginationWrapper<PlayHistory>),
        ["CursorPagingSimplifiedArtistObject"] = typeof(CursorPaginationWrapper<Artist>),
        ["CursorPagedArtists"] = typeof(FollowedArtists),
        ["PagingArtistDiscographyAlbumObject"] = typeof(OffsetPaginationWrapper<SimplifiedAlbum>),
        ["PagingArtistObject"] = typeof(OffsetPaginationWrapper<Artist>),
        ["PagingPlaylistObject"] = typeof(OffsetPaginationWrapper<SimplifiedPlaylist>),
        ["PagingPlaylistTrackObject"] = typeof(OffsetPaginationWrapper<PlaylistTrack>),
        ["PagingSavedAlbumObject"] = typeof(OffsetPaginationWrapper<SavedAlbum>),
        ["PagingSavedEpisodeObject"] = typeof(OffsetPaginationWrapper<SavedEpisode>),
        ["PagingSavedShowObject"] = typeof(OffsetPaginationWrapper<SavedShow>),
        ["PagingSavedTrackObject"] = typeof(OffsetPaginationWrapper<SavedTrack>),
        ["PagingSimplifiedAlbumObject"] = typeof(OffsetPaginationWrapper<SimplifiedAlbum>),
        ["PagingSimplifiedAudiobookObject"] = typeof(OffsetPaginationWrapper<SimplifiedAudiobook>),
        ["PagingSimplifiedChapterObject"] = typeof(OffsetPaginationWrapper<SimplifiedChapter>),
        ["PagingSimplifiedEpisodeObject"] = typeof(OffsetPaginationWrapper<SimplifiedEpisode>),
        ["PagingSimplifiedShowObject"] = typeof(OffsetPaginationWrapper<SimplifiedShow>),
        ["PagingSimplifiedTrackObject"] = typeof(OffsetPaginationWrapper<SimplifiedTrack>),
        ["PagingTrackObject"] = typeof(OffsetPaginationWrapper<Track>),
        ["SearchItems"] = typeof(SearchResult),
    };

    // allOf bases are flattened into their models; the rest only serve deprecated endpoints or fields.
    private static readonly string[] NotModeled =
    [
        "AlbumBase",
        "AudiobookBase",
        "ChapterBase",
        "CursorPagingObject",
        "EpisodeBase",
        "PagingObject",
        "ShowBase",
        "AudioAnalysisObject",
        "AudioFeaturesObject",
        "CategoryObject",
        "ExplicitContentSettingsObject",
        "FollowersObject",
        "Key",
        "LinkedTrackObject",
        "Loudness",
        "Mode",
        "PagingFeaturedPlaylistObject",
        "PublicUserObject",
        "RecommendationSeedObject",
        "RecommendationsObject",
        "SectionObject",
        "SegmentObject",
        "Tempo",
        "TimeIntervalObject",
        "TimeSignature",
    ];

    // Deliberately looser than the spec (nullable, may be missing), as Spotify sends them: albums of
    // local files in playlists, is_playable without a market, shows of audiobooks in playback, and
    // episodes' is_externally_hosted.
    private static readonly (Type Model, string Field)[] Relaxed =
    [
        (typeof(SimplifiedAlbum), "album_type"),
        (typeof(SimplifiedAlbum), "total_tracks"),
        (typeof(SimplifiedAlbum), "href"),
        (typeof(SimplifiedAlbum), "id"),
        (typeof(SimplifiedAlbum), "release_date"),
        (typeof(SimplifiedAlbum), "release_date_precision"),
        (typeof(SimplifiedAlbum), "uri"),
        (typeof(Chapter), "is_playable"),
        (typeof(SimplifiedChapter), "is_playable"),
        (typeof(Episode), "is_playable"),
        (typeof(SimplifiedEpisode), "is_playable"),
        (typeof(Episode), "is_externally_hosted"),
        (typeof(SimplifiedEpisode), "is_externally_hosted"),
        (typeof(SimplifiedShow), "total_episodes"),
    ];

    private static readonly Node Spec = new DeserializerBuilder()
        .Build()
        .Deserialize<Node>(
            File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "open-api-schema.yaml"))
        );

    public static IEnumerable<string> ModeledSchemas() => Models.Keys;

    [Test]
    public async Task EverySchemaIsModeledOrExcluded()
    {
        var schemas = ((Node)Get(Spec, "components", "schemas")!).Keys.Cast<string>();

        await Assert.That(schemas.Except(Models.Keys).Except(NotModeled)).IsEmpty();
    }

    [Test]
    [MethodDataSource(nameof(ModeledSchemas))]
    public async Task ModelMatchesSchema(string schema)
    {
        var (properties, required) = Flatten(Schema(schema));
        var members = Models[schema]
            .GetProperties()
            .ToDictionary(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name);
        var nullability = new NullabilityInfoContext();
        var problems = new List<string>();

        foreach (var (name, property) in properties)
        {
            var deprecated = Get(property, "deprecated") is "true";
            if (!members.TryGetValue(name, out var member))
            {
                if (!deprecated)
                {
                    problems.Add($"{name}: missing");
                }

                continue;
            }

            if (deprecated)
            {
                problems.Add($"{name}: deprecated");
                continue;
            }

            var relaxed = Relaxed.Contains((Models[schema], name));
            var isRequired = required.Contains(name) && !relaxed;
            var isNullable =
                relaxed
                || !isRequired
                || Get(property, "nullable") is "true"
                || NullMention().IsMatch(Get(property, "description") as string ?? "");

            if (member.IsDefined(typeof(RequiredMemberAttribute)) != isRequired)
            {
                problems.Add($"{name}: required should be {isRequired}");
            }

            if (IsNullable(member, nullability) != isNullable)
            {
                problems.Add($"{name}: nullable should be {isNullable}");
            }

            if (TypeProblem(name, property, member.PropertyType) is { } problem)
            {
                problems.Add(problem);
            }
        }

        problems.AddRange(members.Keys.Except(properties.Keys).Select(n => $"{n}: not in spec"));

        await Assert.That(problems).IsEmpty();
    }

    private static string? TypeProblem(string name, Node schema, Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        var reference =
            Get(schema, "$ref") ?? Get((Get(schema, "allOf") as List<object>)?[0], "$ref");
        if (reference is string path)
        {
            var target = path.Split('/')[^1];
            return Models.GetValueOrDefault(target) == type
                ? null
                : $"{name}: {type.Name} isn't {target}";
        }

        if (Get(schema, "oneOf") is not null)
        {
            return type == typeof(PlayableItem) ? null : $"{name}: {type.Name} isn't PlayableItem";
        }

        if (Get(schema, "type") is "array")
        {
            return type.IsArray
                ? TypeProblem($"{name}[]", (Node)Get(schema, "items")!, type.GetElementType()!)
                : $"{name}: {type.Name} isn't an array";
        }

        var fits = Get(schema, "type") switch
        {
            "string" when Get(schema, "format") is "date-time" => type == typeof(DateTime),
            "string" when Get(schema, "enum") is List<object> values => type.IsEnum
                && values.All(v =>
                    Enum.GetNames(type).Contains((string)v, StringComparer.OrdinalIgnoreCase)
                ),
            // Values documented only in prose stay strings, except `type` and `reason`.
            "string" => type == typeof(string) || (type.IsEnum && name is "type" or "reason"),
            "integer" => type == typeof(int) || type == typeof(long),
            "boolean" => type == typeof(bool),
            _ => false,
        };

        return fits ? null : $"{name}: {type.Name} doesn't fit {Get(schema, "type")}";
    }

    private static (Dictionary<string, Node> Properties, HashSet<string> Required) Flatten(
        Node schema
    )
    {
        var properties = new Dictionary<string, Node>();
        var required = new HashSet<string>();

        foreach (Node part in Get(schema, "allOf") as List<object> ?? [])
        {
            var (partProperties, partRequired) = Flatten(
                Get(part, "$ref") is string path ? Schema(path.Split('/')[^1]) : part
            );
            foreach (var (name, property) in partProperties)
            {
                properties[name] = property;
            }

            required.UnionWith(partRequired);
        }

        foreach (var (name, property) in Get(schema, "properties") as Node ?? new Node())
        {
            properties[(string)name] = (Node)property;
        }

        required.UnionWith((Get(schema, "required") as List<object> ?? []).Cast<string>());

        return (properties, required);
    }

    private static Node Schema(string name) =>
        (Node?)Get(Spec, "components", "schemas", name)
        ?? (Node)
            Get(Spec, "components", "responses", name, "content", "application/json", "schema")!;

    private static object? Get(object? node, params string[] path)
    {
        foreach (var key in path)
        {
            node = node is Node map && map.TryGetValue(key, out var value) ? value : null;
        }

        return node;
    }

    private static bool IsNullable(PropertyInfo member, NullabilityInfoContext context) =>
        Nullable.GetUnderlyingType(member.PropertyType) is not null
        || (
            !member.PropertyType.IsValueType
            && context.Create(member).ReadState == NullabilityState.Nullable
        );

    // Some fields say they can be null only in their description, e.g. ShowBase.is_externally_hosted.
    [GeneratedRegex(@"\bnull\b")]
    private static partial Regex NullMention();
}
