using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private GameObject rootCanvas;
    private LobbyController lobbyController;
    private string lobbyName = "My Lobby";
    private string password;
    private bool isPrivate = false;
    private int maxPlayers = 3;
    private GameModes gameMode = GameModes.PropHunt;

    [SerializeField]
    private TMP_InputField lobbyNameInput;
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    private TMP_InputField maxPlayersInput;

    private void Awake()
    {
        lobbyController = rootCanvas.GetComponent<LobbyController>();
    }

    public void CreateLobby()
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>
        {
            { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString(), DataObject.IndexOptions.S1) }
        };

        lobbyController.CreateLobby(lobbyName, password, isPrivate, maxPlayers, lobbyData);
    }

    public void SetLobbyName(string value)
    {
        lobbyNameInput.text = value;
        lobbyName = value;
    }
}
