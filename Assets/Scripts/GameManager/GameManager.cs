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
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(!IsServer)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(players.Count);
    }

    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        string _playerName = "Player " + _netID;
        _player.transform.name = _playerName;
        _player.SetPlayerData(_netID, _playerName, 100);
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

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string p in players.Keys) 
        {
            GUILayout.Label(p + "  -  " + players[p].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
