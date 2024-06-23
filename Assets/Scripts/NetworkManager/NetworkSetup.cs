using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSetup : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            InstantiateGameManager();
        }
    }

    private void InstantiateGameManager()
    {
        if(GameManager.Instance == null)
        {
            GameObject gameManager = Instantiate(gameManagerPrefab);
            gameManager.GetComponent<NetworkObject>().Spawn();
            Debug.Log("GameManager instantiated and spawned");
        }
    }
}
