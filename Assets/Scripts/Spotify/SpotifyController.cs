using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using UnityEngine;

public class SpotifyController : MonoBehaviour 
{
    public static SpotifyController Instance { get; private set; }
    private SpotifyClient _spotify;
    public int UserId;

    private void Awake()
    {
        if (Instance == null)
        {
            _spotify = new SpotifyClient(new Auth(UserId).AccessToken);
            Instance = this;
        }
        else if (Instance != this) Destroy(this);
        DontDestroyOnLoad(this);
    }

    public async Task<bool> GetPlayPauseState()
    {
        CurrentlyPlayingContext context = await _spotify.Player.GetCurrentPlayback();
        return context.IsPlaying;
    }

    public async Task Pause()
    {
        await _spotify.Player.PausePlayback();
    }

    public async Task Play()
    {
        await _spotify.Player.ResumePlayback();
    }

    public async Task TogglePlayPause()
    {
        var currentPlaybackInfo = await _spotify.Player.GetCurrentPlayback();

        if (currentPlaybackInfo.IsPlaying)
            await Pause();
        else
            await Play();
    }

    private async Task Seek(int seconds)
    {
        long ms = seconds * 1000;

        var currentPlaybackInfo = await _spotify.Player.GetCurrentPlayback();
        await _spotify.Player.SeekTo(new PlayerSeekToRequest(currentPlaybackInfo.ProgressMs + ms));
    }

    public async Task FastForward(int seconds)
    {
        await Seek(seconds);
    }

    public async Task Rewind(int seconds)
    {
        await Seek(-seconds);
    }

    public async Task Next()
    {
        await _spotify.Player.SkipNext();
    }

    public async Task Previous()
    {
        await _spotify.Player.SkipPrevious();
    }

    public async Task<List<Song>> GetPlaylist()
    {
        var userPlaylists = await _spotify.Playlists.CurrentUsers();
        if (userPlaylists.Items.Count == 0)
        {
            Debug.Log("No playlists found for the current user.");
            return null;
        }

        var playlistItems = await _spotify.Playlists.GetItems(userPlaylists.Items[0].Id);
        var songs = new List<Song>();

        foreach (var item in playlistItems.Items)
        {
            if (item.Track is FullTrack track)
            {
                songs.Add(new Song()
                {
                    ID = track.Id,
                    Title = track.Name,
                    AlbumID = track.Album.Id,
                    AlbumName = track.Album.Name,
                    ArtistID = track.Artists[0].Id,
                    ArtistName = track.Artists[0].Name,
                    Duration = track.DurationMs / 1000,
                    IsPlayable = track.IsPlayable,
                    IsLocal = track.IsLocal
                });
            }
        }
        return songs;
    }

    public async Task<Song> GetCurrentlyPlayingSong()
    {
        var currentlyPlaying = await _spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());

        if (currentlyPlaying?.Item is FullTrack track)
        {
            return new Song()
            {
                ID = track.Id,
                Title = track.Name,
                Duration = track.DurationMs / 1000,
                ArtistID = track.Artists[0].Id,
                ArtistName = track.Artists[0].Name,
                AlbumID = track.Album.Id,
                AlbumName = track.Album.Name,
                IsPlayable = track.IsPlayable,
                IsLocal = track.IsLocal
            };
        }

        Debug.Log("No track is currently playing.");
        return null;
    }

    public async Task<bool> PlaySearchedSong(string songName)
    {
        var searchResponse = await _spotify.Search.Item(new SearchRequest(SearchRequest.Types.Track, songName));
        await _spotify.Player.AddToQueue(new PlayerAddToQueueRequest(searchResponse.Tracks.Items[0].Uri));
        await _spotify.Player.SkipNext();

        return GetCurrentlyPlayingSong().Result.ID == searchResponse.Tracks.Items[0].Id;
    }
}