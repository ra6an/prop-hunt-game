using Console;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string PLAYER_ID_PREFIX = "Player ";

    [SerializeField]
    public GameObject playersScore;

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    if(!IsServer)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void RegisterPlayerServerRpc(string _netID, PlayerManager _player)
    public void RegisterPlayer(string _netID, string _playerName, int _playerHealth)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        RegisterPlayerServerRpc(_netID, _playerName, _playerHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RegisterPlayerServerRpc(string _netID, string _playerName, int _playerHealth)
    {
        PlayerManager _player = new PlayerManager();
        _player.SetPlayerData(_netID, _playerName, _playerHealth);

        if (!players.ContainsKey(_netID))
        {
            players.Add(_netID, _player);
            Debug.Log("Player registered successfully: " + _netID);
        }

        foreach (var p in players.Values)
        {
            UpdateClientsClientRpc(p.playerData.playerID, p.playerData.playerName, p.playerData.playerHealth);
        }
    }

    [ClientRpc]
    private void UpdateClientsClientRpc(string _netID, string _playerName, int _playerHealth)
    {
        if (IsServer) return;
        if (!players.ContainsKey(_netID))
        {
            PlayerManager _player = new PlayerManager();
            _player.SetPlayerData(_netID, _playerName, _playerHealth);
            players.Add(_netID, _player);
        }
        else
        {
            Debug.LogWarning("Player already exists on client: " + _netID);
        }
    }

    public static void UnRegisterPlayer(string _netID)
    {
        if(players.ContainsKey(_netID))
        {
            players.Remove(_netID);
        }
    }

    public Dictionary<string, PlayerManager> GetPlayersData()
    {
        return players;
    }

    public static PlayerManager GetPlayer(string _netID)
    {
        return players[_netID];
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            Debug.Log("GameManager spawned on server.");
        }
        playersScore = GameObject.Find("PlayersHUD");
    }
}
