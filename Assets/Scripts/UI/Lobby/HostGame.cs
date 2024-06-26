using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class HostGame : MonoBehaviour
{

    public string KEY_START_GAME = "JoinKey";
    [SerializeField]
    private GameObject rootCanvas;
    private LobbyController lobbyController;
    private string lobbyName = "My Lobby";
    private string password;
    private bool isPrivate = false;
    private int maxPlayers = 2;
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

    private void Start()
    {
        lobbyNameInput.text = lobbyName;
        // Ensure the onValueChanged event is hooked up
        lobbyNameInput.onValueChanged.AddListener(SetLobbyName);
    }

    public void CreateLobby()
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>
        {
            { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString(), DataObject.IndexOptions.S1) },
            { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0", DataObject.IndexOptions.S2) }
            //{ "GameStarted", new DataObject(DataObject.VisibilityOptions.Member, "false", DataObject.IndexOptions.S2) },

        };

        lobbyController.CreateLobby(lobbyName, password, isPrivate, maxPlayers, lobbyData);
        rootCanvas.GetComponent<MainMenuController>().OnCreateJoinLobby();
    }

    public void SetLobbyName(string value)
    {
        lobbyNameInput.text = value;
        lobbyName = value;
    }

    public void SetPassword(string value)
    {
        passwordInput.text = value;
        password = value;
    }

    public void SetMaxPlayers(string value)
    {
        maxPlayersInput.text = value;
        //maxPlayers = value;

        switch (value)
        {
            case "2": maxPlayers = 2;
                break;
            case "3": maxPlayers = 3;
                break;
            case "4": maxPlayers = 4;
                break;
            case "5": maxPlayers = 5;
                break;
            case "6": maxPlayers = 6;
                break;
            case "7": maxPlayers = 7;
                break;
            case "8": maxPlayers = 8;
                break;
            default:
                maxPlayers = 2;
                break;
        }
    }
}
