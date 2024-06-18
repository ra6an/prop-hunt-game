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

    public void OnHostGame()
    {
        title.SetActive(false);
        mainMenuOptions.SetActive(false);
        joinGame.SetActive(false);
        options.SetActive(false);
        hostGame.SetActive(true);
    }
    public void OnJoinGame()
    {
        title.SetActive(false);
        mainMenuOptions.SetActive(false);
        joinGame.SetActive(true);
        options.SetActive(false);
        hostGame.SetActive(false);
    }
    public void OnOptions()
    {
        title.SetActive(false);
        mainMenuOptions.SetActive(false);
        joinGame.SetActive(false);
        options.SetActive(true);
        hostGame.SetActive(false);
    }

    public void BackToMainMenu()
    {
        title.SetActive(true);
        mainMenuOptions.SetActive(true);
        joinGame.SetActive(false);
        options.SetActive(false);
        hostGame.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
