using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;
    private GameObject cameraInstance;

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

            //cameraInstance = Instantiate(cameraPrefab);
            //cameraInstance.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            //cameraInstance.transform.SetParent(transform);
            //cameraInstance.transform.localPosition = new Vector3(0f, 1.7f, 0f);

            //transform.GetComponent<PlayerLook>().cam = cameraInstance.GetComponent<Camera>();
            //transform.GetComponent<PlayerShoot>().cam = cameraInstance.GetComponent<Camera>();
            //componentsToDisable.add
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsLocalPlayer) return;
        string _netID = NetworkManager.Singleton.LocalClientId.ToString();
        Debug.Log(_netID);
        PlayerManager _player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(_netID, _player);
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

        GameManager.UnRegisterPlayer(transform.name);
    }
}
