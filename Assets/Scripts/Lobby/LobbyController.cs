using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public enum GameModes
{
    PropHunt,
    Tag
}

public class LobbyController : MonoBehaviour
{
    public string KEY_START_GAME = "JoinKey";
    private Lobby hostLobby;
    public Lobby joinedLobby;
    private float heartbeathTimer;
    private string playerName;
    public string playerId;
    public event EventHandler<EventArgs> OnGameStarted;

    private float lobbyUpdateTimer;

    // Start is called before the first frame update
    private async void Start()
    {
        playerName = $"Player {UnityEngine.Random.Range(10, 99)}";
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            playerId = AuthenticationService.Instance.PlayerId;
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeathTimer -= Time.deltaTime;
            if(heartbeathTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeathTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                CheckGameStartStatus();
            }
        }
    }

    public async void CreateLobby(string _lobbyName, string _password, bool _isPrivate, int _maxPlayers, Dictionary<string, DataObject> _lobbyOptions)
    {
        try
        {
            string lobbyName = _lobbyName != String.Empty ? _lobbyName : "My Lobby";
            int maxPlayers = _maxPlayers < 2 || _maxPlayers > 8 ? 2 : _maxPlayers;

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = _isPrivate,
                Player = GetPlayer(),
                Data = _lobbyOptions,
                Password = _password,
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + ", lobby code: " + lobby.LobbyCode);

            PrintPlayers(hostLobby);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions query = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    new QueryFilter(QueryFilter.FieldOptions.S1, "Prop Hunt", QueryFilter.OpOptions.EQ)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(query);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby l in queryResponse.Results)
            {
                Debug.Log(l.Name + " " + l.MaxPlayers + " " + l.Data["GameMode"]);
            }
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode(string _lobbyCode, string _lobbyPassword)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
                Password = _lobbyPassword
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(_lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;

            Debug.Log("Lobby joined: " + joinedLobby.Name + "; maxPlayers: " + joinedLobby.MaxPlayers);
            PrintPlayers(joinedLobby);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
        };
    }

    public void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name);

        // Check if the 'GameMode' key exists
        if (lobby.Data.ContainsKey("GameMode"))
        {
            Debug.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value);
        }
        else
        {
            Debug.LogWarning("GameMode key not found in lobby data.");
        }

        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newPlayerName) }
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private bool IsLobbyHost()
    {
        bool isHost = joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        return isHost;
    }

    // TEST ZA GAME START

    public async void StartGame()
    {
        if (joinedLobby == null)
        {
            Debug.LogError("No lobby found to start the game.");
            return;
        }

        if (!IsLobbyHost()) return;

        try
        {
            Debug.Log("Start Game");

            string relayCode = await RelayController.Instance.CreateRelay(joinedLobby.MaxPlayers);

            Lobby _lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) } 
                }
            });

            joinedLobby = _lobby;

            NetworkManager.Singleton.SceneManager.LoadScene("HorrorScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            HandleCanvas();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void StartGameServerRpc()
    //{
    //    //NetworkManager.Singleton.SceneManager.LoadScene("HorrorScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    //}

    private void CheckGameStartStatus()
    {
        if (joinedLobby == null) return;

        if (joinedLobby.Data[KEY_START_GAME].Value != "0")
        {
            if(!IsLobbyHost())
            {
                Debug.Log("Setupa Klijenta za game.");
                RelayController.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                //NetworkManager.Singleton.SceneManager.LoadScene("HorrorScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
                HandleCanvas();
            }

            joinedLobby = null;

            //OnGameStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleCanvas()
    {
        MainCanvasController.Instance.ShowMainMenuCanvas(false);
    }
}
