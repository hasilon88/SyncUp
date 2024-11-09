using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using UnityEngine;

public class TokenData
{
    public string Id { get; set; } // Unique ID for the user
    public string Username { get; set; } // Non-unique Username for display
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}

public class Auth
{
    private string _accessToken;
    public string AccessToken => _accessToken;

    private string _refreshToken;
    private DateTime _expiration;

    private readonly ConfigManager _config;
    private static EmbedIOAuthServer _server;

    /// <summary>
    /// Private constructor for the Auth class, used to initialize configuration settings.
    /// This constructor is only accessible within the class itself and is intended
    /// to be called by the asynchronous method CreateAsync.
    /// </summary>
    /// <param name="id">The unique ID of the user for whom the configuration is being set up.</param>
    private Auth()
    {
        _config = new ConfigManager(Path.Combine(Application.dataPath, "Scripts/Spotify/Data/spotify.config"));
    }

    /// <summary>
    /// Asynchronous method to create and initialize an instance of the Auth class.
    /// This method initializes the instance with a specified user ID and retrieves or refreshes
    /// the user's token before returning a fully initialized Auth object.
    /// </summary>
    /// <param name="id">The unique ID of the user for whom the Auth instance is created.</param>
    /// <returns>An initialized Auth instance associated with the specified user.</returns>
    public static async Task<Auth> CreateAsync(string id)
    {
        var instance = new Auth();
        await instance.GetOrRefreshToken(id);
        return instance;
    }

    /// <summary>
    /// Starts the authorization process to obtain a new authorization code for a specific user.
    /// This method opens a browser for the user to log in and authorize the app.
    /// </summary>
    private async Task GetNewAuthorizationCode()
    {
        _server = new EmbedIOAuthServer(new Uri(
            _config.GetString("SERVER_REDIRECT_URI")),
            _config.GetInt("SERVER_PORT")
        );

        await _server.Start();

        _server.AuthorizationCodeReceived += async (sender, response) => await OnAuthorizationCodeReceived(sender, response);
        _server.ErrorReceived += async (sender, error, state) => await OnErrorReceived(sender, error, state);

        var request = new LoginRequest(_server.BaseUri, _config.GetString("CLIENT_ID"), LoginRequest.ResponseType.Code)
        {
            Scope = new List<string>
            {
                Scopes.UgcImageUpload,
                Scopes.UserReadPlaybackState,
                Scopes.UserModifyPlaybackState,
                Scopes.UserReadCurrentlyPlaying,
                Scopes.Streaming,
                Scopes.AppRemoteControl,
                Scopes.UserReadEmail,
                Scopes.UserReadPrivate,
                Scopes.PlaylistReadCollaborative,
                Scopes.PlaylistModifyPublic,
                Scopes.PlaylistReadPrivate,
                Scopes.PlaylistModifyPrivate,
                Scopes.UserLibraryModify,
                Scopes.UserLibraryRead,
                Scopes.UserTopRead,
                Scopes.UserReadPlaybackPosition,
                Scopes.UserReadRecentlyPlayed,
                Scopes.UserFollowRead,
                Scopes.UserFollowModify
            }
        };

        var uri = request.ToUri();
        try
        {
            BrowserUtil.Open(uri);
        }
        catch (Exception ex)
        {
            Debug.Log($"Unable to open URL, manually open: {uri},\n Exception: {ex}");
        }
    }

    /// <summary>
    /// Handles any errors that occur during the authorization process.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="error">The error message received.</param>
    /// <param name="state">The state of the authorization request.</param>
    private async Task OnErrorReceived(object sender, string error, string state)
    {
        Debug.LogError($"Error received: {error}");
        await Task.CompletedTask;
    }

    /// <summary>
    /// Handles the event when the authorization code is received from Spotify.
    /// This method exchanges the code for an access token and refresh token.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="response">The response containing the authorization code.</param>
    private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
    {
        var tokenResponse = await new OAuthClient(SpotifyClientConfig.CreateDefault()).RequestToken(
            new AuthorizationCodeTokenRequest(
                _config.GetString("CLIENT_ID"),
                _config.GetString("CLIENT_SECRET"),
                response.Code,
                new Uri(_config.GetString("SERVER_REDIRECT_URI"))
            )
        );
        
        await _server.Stop();
        
        _accessToken = tokenResponse.AccessToken;
        _refreshToken = tokenResponse.RefreshToken;
        _expiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

        var profile = await GetUserProfile(_accessToken);
        
        SaveTokenData(profile.Id, profile.DisplayName, _accessToken, _refreshToken, _expiration);
    }

    private async Task<PrivateUser> GetUserProfile(string accessToken)
    {
        var spotifyClient = new SpotifyClient(accessToken);
        return await spotifyClient.UserProfile.Current();
    }

    /// <summary>
    /// Retrieves an existing token from storage or refreshes it if expired.
    /// If no valid token is found, the authorization process starts to get a new one.
    /// </summary>
    /// <param name="id">The unique ID of the user whose token is being fetched or refreshed.</param>
    private async Task GetOrRefreshToken(string id)
    {
        if (id == "")
        {
            await GetNewAuthorizationCode();
            return;
        }
        
        var tokenDataList = LoadTokenData();
        var userToken = tokenDataList.Find(u => u.Id == id);

        if (userToken != null && !IsTokenExpired(userToken))
        {
            _accessToken = userToken.AccessToken;
            _refreshToken = userToken.RefreshToken;
            _expiration = userToken.Expiration;
        }
        else if (userToken != null && IsTokenExpired(userToken))
        {
            await RefreshToken(userToken);
        }
        else
        {
            await GetNewAuthorizationCode();
        }
    }

    /// <summary>
    /// Refreshes the Spotify access token using the provided refresh token.
    /// </summary>
    /// <param name="tokenData">An object containing the user's token data, including the refresh token and user ID.</param>
    private async Task RefreshToken(TokenData tokenData)
    {
        try
        {
            var refreshRequest = new AuthorizationCodeRefreshRequest(_config.GetString("CLIENT_ID"), _config.GetString("CLIENT_SECRET"), tokenData.RefreshToken);
            var tokenResponse = await new OAuthClient(SpotifyClientConfig.CreateDefault()).RequestToken(refreshRequest);

            _accessToken = tokenResponse.AccessToken;
            _expiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

            SaveTokenData(tokenData.Id, tokenData.Username, _accessToken, _refreshToken, _expiration);

            Debug.Log("Token refreshed successfully.");
        }
        catch (APIException e)
        {
            Debug.LogError($"Failed to refresh token: {e.Message}");
            await GetNewAuthorizationCode();
        }
    }

    /// <summary>
    /// Checks whether the stored token for a user has expired.
    /// </summary>
    /// <param name="tokenData">The token data to check.</param>
    /// <returns>True if the token has expired, false otherwise.</returns>
    private bool IsTokenExpired(TokenData tokenData)
    {
        return DateTime.Now >= tokenData.Expiration;
    }

    /// <summary>
    /// Saves the token data for a user. If the JSON file does not exist, it is created.
    /// If the user already has token data, it updates the existing entry; otherwise, it adds a new entry.
    /// </summary>
    /// <param name="id">The unique ID of the user whose token data is being saved.</param>
    /// <param name="username">The display username of the user.</param>
    /// <param name="accessToken">The access token to be saved.</param>
    /// <param name="refreshToken">The refresh token to be saved.</param>
    /// <param name="expiration">The expiration date and time of the access token.</param>
    private void SaveTokenData(string id, string username, string accessToken, string refreshToken, DateTime expiration)
    {
        var tokenDataList = LoadTokenData();
        var existingUserToken = tokenDataList.Find(u => u.Id == id);

        if (existingUserToken != null)
        {
            existingUserToken.AccessToken = accessToken;
            existingUserToken.RefreshToken = refreshToken;
            existingUserToken.Expiration = expiration;
        }
        else
        {
            tokenDataList.Add(new TokenData
            {
                Id = id,
                Username = username,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = expiration
            });
        }

        File.WriteAllText(_config.GetString("USERS_PATH"), JsonConvert.SerializeObject(tokenDataList, Formatting.Indented));
    }

    /// <summary>
    /// Loads the token data from a JSON file.
    /// </summary>
    /// <returns>A list of token data.</returns>
    private List<TokenData> LoadTokenData()
    {
        if (!File.Exists(_config.GetString("USERS_PATH")))
            return new List<TokenData>();

        var json = File.ReadAllText(_config.GetString("USERS_PATH"));
        return JsonConvert.DeserializeObject<List<TokenData>>(json);
    }
}
