using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Chapters;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IChaptersService
{
    Task<Chapter> GetChapter(string chapterId, CancellationToken cancellationToken = default);
}
