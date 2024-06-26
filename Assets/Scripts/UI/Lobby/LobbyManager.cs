using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Lobby currentLobby;

    public async void StartGame()
    {
        if(currentLobby == null)
        {
            Debug.LogError("No lobby found to start the game.");
            return;
        }

        try
        {
            var updatedLobby = await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameStarted", new DataObject(DataObject.VisibilityOptions.Public, "true") }
                }
            });

            currentLobby = updatedLobby;
            StartGameServerRpc();
        } catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("HorrorScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void SetCurrentLobby(Lobby lobby)
    {
        currentLobby = lobby;
    }
}
