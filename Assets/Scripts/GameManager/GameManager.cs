using Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string PLAYER_ID_PREFIX = "Player ";

    [SerializeField]
    public GameObject playersScore;
    [SerializeField]
    public SpawnPoints spawnPoints;
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject playersParent;

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    private static List<ulong> playersToBeSpawned = new List<ulong>();

    private bool gameInitialized = false;

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

    private void Start()
    {
        //GameObject go = Instantiate(new GameObject());
        //go.transform.position = new Vector3(0f, 0f, 0f);
        //go.name = "PLAYERS";
        //playersParent = go;
    }

    private void Update()
    {
        if (!gameInitialized) return;
        if(NetworkManager.Singleton.ConnectedClientsList.Count > players.Count)
        {
            foreach(var p in NetworkManager.Singleton.ConnectedClientsList)
            {
                string pId = p.ClientId.ToString();
                if(!players.ContainsKey(pId))
                {
                    SpawnPlayer(p.ClientId);
                }
            }
        }
    }

    //public void RegisterPlayerServerRpc(string _netID, PlayerManager _player)
    public void RegisterPlayer(string _netID, string _playerName, int _playerHealth)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        RegisterPlayerServerRpc(_netID, _playerName, _playerHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RegisterPlayerServerRpc(string _netID, string _playerName, int _playerHealth)
    {
        //PlayerManager _player = new PlayerManager();
        //_player.SetPlayerData(_netID, _playerName, _playerHealth);

        //if (!players.ContainsKey(_netID))
        //{
        //    players.Add(_netID, _player);
        //    //Debug.Log("Player registered successfully: " + _netID);
        //}

        //foreach (var p in players.Values)
        //{
        //    UpdateClientsClientRpc(p.playerData.playerID, p.playerData.playerName, p.playerData.playerHealth);
        //}
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
            Debug.Log("OVO JE SERVER!!!!");
            //Debug.Log("GameManager spawned on server.");
            StartCoroutine(SetupGame());
        }
        playersScore = GameObject.Find("PlayersHUD");
    }

    private IEnumerator SetupGame()
    {
        yield return new WaitForSeconds(2);

        Debug.Log("Setuping game.... (SpawningPlayers): " + NetworkManager.Singleton.ConnectedClientsList.Count);
        foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            SpawnPlayer(client.ClientId);
        }

        gameInitialized = true;
    }

    private void SpawnPlayer(ulong clientId)
    {
        Vector3 _spawnPoint = spawnPoints.points[UnityEngine.Random.Range(0, spawnPoints.points.Count)];

        GameObject playerObject = Instantiate(playerPrefab, _spawnPoint, Quaternion.identity);
        NetworkObject networkObject = playerObject.GetComponent<NetworkObject>();

        if(networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId);
        }

        string pId = clientId.ToString();
        PlayerManager playerManager = playerObject.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.SetPlayerData(pId, "Player " + pId, 100);
            players.Add(pId, playerManager);
        }

        //playerObject.name = clientId.ToString();
        //string pid = clientId.ToString();
        //RegisterPlayer(pid, "Player_" + pid, 100);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        Debug.Log("Sending to server");
        playersToBeSpawned.Add(clientId);
    }
}
