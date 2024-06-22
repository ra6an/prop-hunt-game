using Console;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance { get; private set; }

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

    private void Awake()
    {
        //if(Instance == null)
        //{
        //    Instance = this;
        //}
    }

    private void Start()
    {
        //playerInput = new PlayerInput();
        //dev = playerInput.Developer;
    }

    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

    }

    public static void UnRegisterPlayer(string name)
    {
        if(players.ContainsKey(name))
        {
            players.Remove(name);
        }
    }

    public static PlayerManager GetPlayer(string _playerID)
    {
        return players[_playerID];
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
