using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinGame : MonoBehaviour
{
    [SerializeField]
    private GameObject rootCanvas;
    private LobbyController lobbyController;

    [SerializeField]
    private TMP_InputField lobbyCodeInput;
    [SerializeField]
    private TMP_InputField lobbyPasswordInput;

    private string lobbyCode;
    private string joinLobbyPassword;

    private void Awake()
    {
        lobbyController = rootCanvas.GetComponent<LobbyController>();
    }

    public void JoinLobbyByCode()
    {
        lobbyController.JoinLobbyByCode(lobbyCode, joinLobbyPassword);
        rootCanvas.GetComponent<MainMenuController>().OnCreateJoinLobby();
    }

    public void SetLobbyCode(string value)
    {
        lobbyCodeInput.text = value;
        lobbyCode = value;
    }

    public void SetLobbyPassword(string value)
    {
        lobbyPasswordInput.text = value;
        joinLobbyPassword = value;
    }
}
