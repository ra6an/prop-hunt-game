using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCanvas;
    private LobbyController lobbyController;

    private Lobby joinedLobby;

    [Header("Lobby")]
    [SerializeField]
    private GameObject lobbyCode;
    [SerializeField]
    private GameObject lobbyGameMode;
    [SerializeField]
    private GameObject lobbyName;
    [SerializeField]
    private GameObject lobbyMaxPlayers;
    [SerializeField]
    private GameObject lobbyMap;
    [SerializeField]
    private GameObject lobbyPlayersTitle;
    [SerializeField]
    private GameObject startLobbyButton;
    [SerializeField]
    private List<GameObject> players;

    private bool isHost;

    private void Awake()
    {
        lobbyController = mainMenuCanvas.GetComponent<LobbyController>();
    }

    private void Start()
    {
        isHost = false;
    }

    private void Update()
    {
        if (joinedLobby == null)
        {
            joinedLobby = lobbyController.joinedLobby;

            return;
        }

        joinedLobby = lobbyController.joinedLobby;
        isHost = joinedLobby.HostId == lobbyController.playerId;
        if(isHost) startLobbyButton.SetActive(true);
        
        SetLobbyInfo();
        SetLobbyPlayers();
    }

    private void SetLobbyInfo()
    {
        lobbyCode.GetComponent<TMP_Text>().text = joinedLobby.LobbyCode;
        lobbyGameMode.GetComponent<TMP_Text>().text = "Prop hunt";
        lobbyName.GetComponent<TMP_Text>().text = joinedLobby.Name;
        lobbyMaxPlayers.GetComponent<TMP_Text>().text = joinedLobby.MaxPlayers.ToString();
        lobbyMap.GetComponent<TMP_Text>().text = "Hunted House";
    }

    private void SetLobbyPlayers()
    {
        lobbyPlayersTitle.GetComponent<TMP_Text>().text = $"{joinedLobby.Players.Count} / {joinedLobby.MaxPlayers} Players";
        for(int i = 0; i < players.Count; i++)
        {
            GameObject curr = players[i];

            if (i >= joinedLobby.Players.Count)
            {
                curr.SetActive(false);
            }
            else
            {
                var currPlayer = joinedLobby.Players[i];
                if (currPlayer == null)
                {
                    curr.SetActive(false);
                }
                else
                {
                    curr.GetComponent<SinglePlayerLobbyInfo>().SetPlayer(currPlayer.Id, currPlayer.Data["PlayerName"].Value);
                    curr.SetActive(true);
                }
            }
        }
    }

    public void LeaveLobby()
    {
        mainMenuCanvas.GetComponent<LobbyController>().LeaveLobby();
        mainMenuCanvas.GetComponent<MainMenuController>().BackToMainMenu();
    }
}
