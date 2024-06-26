using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasController : MonoBehaviour
{
    public static MainCanvasController Instance { get; private set; }

    [SerializeField]
    private GameObject mainMenuCanvas;
    [SerializeField]
    private GameObject gameStartedCanvas;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowMainMenuCanvas(bool show)
    {
        mainMenuCanvas.SetActive(show);
    }

    public void ShowGameStartedCanvas(bool show)
    {
        gameStartedCanvas.SetActive(show);
    }
}
