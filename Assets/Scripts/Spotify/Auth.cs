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
    public string UserId { get; set; }
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
    /// <param name="userId">The ID of the user for whom the configuration is being set up.</param>
    private Auth(int userId)
    {
        _config = new ConfigManager(Path.Combine(Application.dataPath, "Scripts/Spotify/Data/spotify.config"));
    }

    /// <summary>
    /// Asynchronous method to create and initialize an instance of the Auth class.
    /// This method initializes the instance with a specified user ID and retrieves or refreshes
    /// the user's token before returning a fully initialized Auth object.
    /// </summary>
    /// <param name="userId">The ID of the user for whom the Auth instance is created.</param>
    /// <returns>An initialized Auth instance associated with the specified user.</returns>
    public static async Task<Auth> CreateAsync(int userId)
    {
        var instance = new Auth(userId);
        await instance.GetOrRefreshToken(userId.ToString());
        return instance;
    }


    /// <summary>
    /// Starts the authorization process to obtain a new authorization code for a specific user.
    /// This method opens a browser for the user to log in and authorize the app.
    /// </summary>
    /// <param name="userId">The ID of the user requesting authorization.</param>
    private async Task GetNewAuthorizationCode(string userId)
    {
        _server = new EmbedIOAuthServer(new Uri(
            _config.GetString("SERVER_REDIRECT_URI")),
            _config.GetInt("SERVER_PORT")
        );

        await _server.Start();

        _server.AuthorizationCodeReceived += async (sender, response) => await OnAuthorizationCodeReceived(sender, response, userId);
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
            Debug.Log($"Unable to open URL, manually open: {uri}, Exception: {ex}");
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
    /// <param name="userId">The ID of the user for whom the code is received.</param>
    private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response, string userId)
    {
        var tokenResponse = await new OAuthClient(SpotifyClientConfig.CreateDefault()).RequestToken(
            new AuthorizationCodeTokenRequest(
                _config.GetString("CLIENT_ID"),
                _config.GetString("CLIENT_SECRET"),
                response.Code,
                new Uri(_config.GetString("SERVER_REDIRECT_URI"))
            )
        );

        _accessToken = tokenResponse.AccessToken;
        _refreshToken = tokenResponse.RefreshToken;
        _expiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

        SaveTokenData(userId, _accessToken, _refreshToken, _expiration);

        await _server.Stop();
    }

    /// <summary>
    /// Retrieves an existing token from storage or refreshes it if expired.
    /// If no valid token is found, the authorization process starts to get a new one.
    /// </summary>
    /// <param name="userId">The ID of the user whose token is being fetched or refreshed.</param>
    private async Task GetOrRefreshToken(string userId)
    {
        var tokenDataList = LoadTokenData();
        var userToken = tokenDataList.Find(u => u.UserId == userId);

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
            await GetNewAuthorizationCode(userId);
        }
    }

    /// <summary>
    /// Refreshes the Spotify access token using the provided refresh token.
    /// </summary>
    /// <param name="tokenData">An object containing the user's token data, including the refresh token and user ID.</param>
    /// <remarks>
    /// This method attempts to renew the access token by sending a refresh request to the Spotify OAuth service.
    /// If successful, it updates the access token and expiration time, then saves the updated token data.
    /// If the refresh fails, it logs an error message and triggers a new authorization code request to obtain a fresh access and refresh token.
    /// </remarks>
    /// <exception cref="APIException">Logs an error and initiates re-authorization if token refresh fails due to an API exception.</exception>
    /// <returns>Task representing the asynchronous operation.</returns>
    private async Task RefreshToken(TokenData tokenData)
    {
        try
        {
            var refreshRequest = new AuthorizationCodeRefreshRequest(_config.GetString("CLIENT_ID"), _config.GetString("CLIENT_SECRET"), tokenData.RefreshToken);
            var tokenResponse = await new OAuthClient(SpotifyClientConfig.CreateDefault()).RequestToken(refreshRequest);

            _accessToken = tokenResponse.AccessToken;
            _expiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

            SaveTokenData(tokenData.UserId, _accessToken, _refreshToken, _expiration);

            Debug.Log("Token refreshed successfully.");
        }
        catch (APIException e)
        {
            Debug.LogError($"Failed to refresh token: {e.Message}");
            await GetNewAuthorizationCode(tokenData.UserId);
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
    /// <param name="userId">The ID of the user whose token data is being saved.</param>
    /// <param name="accessToken">The access token to be saved.</param>
    /// <param name="refreshToken">The refresh token to be saved.</param>
    /// <param name="expiration">The expiration date and time of the access token.</param>
    private void SaveTokenData(string userId, string accessToken, string refreshToken, DateTime expiration)
    {
        string userFilePath = _config.GetString("USERS_PATH");

        if (!File.Exists(userFilePath))
        {
            var emptyTokenList = new List<TokenData>();
            string jsonOutput = JsonConvert.SerializeObject(emptyTokenList, Formatting.Indented);
            File.WriteAllText(userFilePath, jsonOutput);
        }

        var tokenDataList = LoadTokenData();
        var existingUser = tokenDataList.Find(u => u.UserId == userId);

        if (existingUser != null)
        {
            existingUser.AccessToken = accessToken;
            existingUser.RefreshToken = refreshToken;
            existingUser.Expiration = expiration;
        }
        else
        {
            tokenDataList.Add(new TokenData
            {
                UserId = userId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = expiration
            });
        }

        string updatedJsonOutput = JsonConvert.SerializeObject(tokenDataList, Formatting.Indented);
        File.WriteAllText(userFilePath, updatedJsonOutput);
    }
    
    /// <summary>
    /// Loads the stored token data for all users from persistent storage.
    /// </summary>
    /// <returns>A list of token data for all users.</returns>
    private List<TokenData> LoadTokenData()
    {
        if (!File.Exists(_config.GetString("USERS_PATH")))
            return new List<TokenData>();

        var json = File.ReadAllText(_config.GetString("USERS_PATH"));

        try
        {
            return JsonConvert.DeserializeObject<List<TokenData>>(json);
        }
        catch (JsonSerializationException)
        {
            Debug.LogError("JSON format is incorrect. Please check the users.json file (" + _config.GetString("USERS_PATH") + ").");
            return new List<TokenData>();
        }
    }
}