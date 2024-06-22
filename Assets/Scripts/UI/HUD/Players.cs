using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    [SerializeField]
    private GameObject hunter;
    [SerializeField]
    private GameObject playersList;
    [SerializeField]
    private GameObject singlePlayerInfoPrefab;

    public void SetPlayers(Dictionary<string, PlayerManager> listOfPlayers)
    {
        foreach (KeyValuePair<string, PlayerManager> entry in listOfPlayers)
        {
            PlayerManager playerManager = entry.Value;
            PlayerData playerData = playerManager.playerData;

            GameObject go = Instantiate(singlePlayerInfoPrefab);
            go.transform.SetParent(playersList.transform, false);
            go.GetComponent<SinglePlayerInfo>().SetPlayerInfo(playerData.playerName, 0);
        }
    }

    internal void RemovePlayers()
    {
        foreach (Transform child in playersList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
