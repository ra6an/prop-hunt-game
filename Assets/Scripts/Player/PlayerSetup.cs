using System.Collections;
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
        if(!IsHost)
        {
            
        }
    }

    private IEnumerator WaitForGameManager()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // Wait for the next frame
        }

        string _netID = NetworkManager.Singleton.LocalClientId.ToString();
        //Debug.Log(_netID);
        //PlayerManager _player = GetComponent<PlayerManager>();
        string _playerName = "Player " + _netID;
        GameManager.Instance.RegisterPlayer(_netID, _playerName, 100);
    }

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
