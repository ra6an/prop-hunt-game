using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;

    [SerializeField]
    Behaviour[] componentsToDisable;


    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    
    Camera sceneCamera;

    private void Awake()
    {
        //if(IsLocalPlayer)
        //{
        //    gameObject.GetComponentInChildren<Camera>()
        //}
    }

    private void Start()
    {
        if(!IsLocalPlayer)
        {
            DisableComponents();
            AssignRemotePlayer();
        }
        else
        {
            sceneCamera = Camera.main;
            //sceneCamera = cameraPrefab.GetComponent<Camera>();
            if(sceneCamera != null )
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsLocalPlayer) return;
        
        StartCoroutine(WaitForGameManager());
    }

    private IEnumerator WaitForGameManager()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // Wait for the next frame
        }

        string _netID = NetworkManager.Singleton.LocalClientId.ToString();
        
        string _playerName = "Player " + _netID;
        GameManager.Instance.RegisterPlayer(_netID, _playerName, 100);
        
        //if(IsLocalPlayer)
        //{
        //}
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void SetPositionServerRpc(Vector3 position)
    //{
    //    transform.position = position;
    //    SetPositionClientRpc(position);
    //}

    //[ClientRpc]
    //private void SetPositionClientRpc(Vector3 position)
    //{
    //    if (!IsLocalPlayer)
    //    {
    //        transform.position = position;
    //    }
    //}

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsLocalPlayer) return;
        string _netID = NetworkManager.Singleton.LocalClientId.ToString();
        GameManager.UnRegisterPlayer(_netID);
    }

    void AssignRemotePlayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
        //cameraPrefab.SetActive(false);
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        //GameManager.UnRegisterPlayer(transform.name);
    }
}
