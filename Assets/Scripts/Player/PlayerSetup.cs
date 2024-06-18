using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    //[SerializeField]
    //public NetworkVariable<FixedString64Bytes> playerName;


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
            AssignRemoteRaley();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null )
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        //StartCoroutine(RenamePlayerHandler());
    }

    //private IEnumerator RenamePlayerHandler()
    //{
    //        yield return new WaitUntil(() => NetworkManager.Singleton.IsConnectedClient);
    //    if (IsLocalPlayer)
    //    {
    //        Debug.Log("Teest");
    //        RenamePlayerServerRpc($"Player_{NetworkManager.Singleton.LocalClientId}");
    //    }
    //}

    //[ServerRpc]
    //private void RenamePlayerServerRpc(string newPlayerName)
    //{
    //    playerName.Value = newPlayerName;
    //}

    //private void OnPlayerNameChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    //{
    //    Debug.Log("Renamea playera");
    //    transform.gameObject.name = newValue.ToString();
    //    playerNameDisplay.text = newValue.ToString();
    //}

    //public override void OnStartClient() { }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        string _netID = NetworkManager.Singleton.LocalClientId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    [ServerRpc]
    void PlayerLoggedServerRpc()
    {
        //Debug.Log(NetworkManager.Singleton.LocalClientId + " Is Online!");
    }

    void AssignRemoteRaley()
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
    //private void OnEnable()
    //{
    //    playerName = new NetworkVariable<FixedString64Bytes>(new FixedString64Bytes(""));
    //    playerName.OnValueChanged += OnPlayerNameChanged;
    //}

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
        //playerName.OnValueChanged -= OnPlayerNameChanged;
    }
}
