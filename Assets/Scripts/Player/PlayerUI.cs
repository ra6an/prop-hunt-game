using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    private GameObject playersScore;
    // Start is called before the first frame update

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if(canvas != null )
        {
            promptText = canvas.transform.Find("PromptText").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        playersScore = GameObject.Find("PANELS").GetComponent<Panels>().playersScore;
    }

    private void Update()
    {
        if (playersScore == null)
        {
            GameObject canvas = GameObject.Find("PANELS");
            if (playersScore != null)
            {
                playersScore = canvas.GetComponent<Panels>().playersScore;
            }
        }

        if(promptText == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                promptText = canvas.transform.Find("PromptText").GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void UpdateText(string promptMessage)
    {
        if (!promptText) return;
        promptText.text = promptMessage;
    }

    public void ShowPlayersScore(bool _show)
    {
        if (playersScore == null) return;

        if(_show)
        {
            playersScore.GetComponent<Players>().SetPlayers(GameManager.Instance.GetPlayersData());
        } else
        {
            playersScore.GetComponent<Players>().RemovePlayers();
        }
        playersScore.SetActive(_show);
    }
}
