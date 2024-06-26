using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject title;
    [SerializeField]
    private GameObject mainMenuOptions;
    [SerializeField]
    private GameObject hostGame;
    [SerializeField]
    private GameObject joinGame;
    [SerializeField]
    private GameObject options;
    [SerializeField]
    private GameObject lobby;

    private void CloseEverything()
    {
        title.SetActive(false);
        mainMenuOptions.SetActive(false);
        joinGame.SetActive(false);
        hostGame.SetActive(false);
        options.SetActive(false);
        lobby.SetActive(false);
    }

    public void OnHostGame()
    {
        CloseEverything();
        hostGame.SetActive(true);
    }
    public void OnJoinGame()
    {
        CloseEverything();
        joinGame.SetActive(true);
    }
    public void OnOptions()
    {
        CloseEverything();
        options.SetActive(true);
    }

    public void BackToMainMenu()
    {
        CloseEverything();
        title.SetActive(true);
        mainMenuOptions.SetActive(true);
    }

    public void OnCreateJoinLobby()
    {
        CloseEverything();
        lobby.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
