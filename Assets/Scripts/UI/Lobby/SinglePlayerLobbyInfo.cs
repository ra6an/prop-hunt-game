using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerLobbyInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject playerId;
    [SerializeField]
    private GameObject playerName;
    [SerializeField]
    private GameObject playerImage;

    public void SetPlayer(string id, string name)
    {
        playerId.GetComponent<TMP_Text>().text = id;
        playerName.GetComponent<TMP_Text>().text = name;
    }
}
