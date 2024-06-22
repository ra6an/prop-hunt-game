using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData
{
    public string playerID;
    public string playerName;
    public int playerHealth;
}

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;

    private void Awake()
    {
        playerData = new PlayerData();
    }

    public void SetPlayerData(string id, string name, int health)
    {
        playerData.playerID = id;
        playerData.playerName = name;
        playerData.playerHealth = health;
    }
}
