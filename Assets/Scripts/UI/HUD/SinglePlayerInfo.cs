using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI score;

    public void SetPlayerInfo(string _playerName, int _score)
    {
        playerName.text = _playerName;
        score.text = _score.ToString();
    }
}
