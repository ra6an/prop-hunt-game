using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData
{
    public string playerName;
    public int playerHealth;
    //public Vector3 playerPosition;
    // Add more fields as needed
}

public class PlayerManager : MonoBehaviour
{
    //public NetworkVariable<PlayerData> playerData = new NetworkVariable<PlayerData>(new PlayerData());

    private void Start()
    {
        
    }

    //private void OnDestroy()
    //{
    //    if(IsLocalPlayer)
    //    {
    //        string _playerId = NetworkManager.Singleton.LocalClientId.ToString();
    ////        GameManager.Instance.UnRegisterPlayer(_playerId);
    //    }
    //}

    //private void InitializePlayerData()
    //{
    //    PlayerData initialData = new PlayerData
    //    {
    //        playerName = "Player" + NetworkManager.Singleton.ConnectedClients.Count,
    //        playerHealth = 100,
    //        playerPosition = Vector3.zero
    //    };

    //    playerData.Value = initialData;
    //}

    // Example RPC to handle player movement updates
    //[ServerRpc]
    //public void SetPlayerPositionServerRpc(Vector3 newPosition)
    //{
    //    PlayerData newData = playerData.Value; // Get current data
    //    newData.playerPosition = newPosition; // Modify the field
    //    playerData.Value = newData; // Set modified data
    //}

    // Example RPC to handle player health updates
    //[ServerRpc]
    //public void SetPlayerHealthServerRpc(int newHealth)
    //{
    //    PlayerData newData = playerData.Value; // Get current data
    //    newData.playerHealth = newHealth; // Modify the field
    //    playerData.Value = newData; // Set modified data
    //}
}
