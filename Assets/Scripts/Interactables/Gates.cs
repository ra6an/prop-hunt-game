using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gates : Interactable
{
    private Animator anim;
    private NetworkVariable<bool> openDoor = new(false);

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void Interact()
    {
        if(IsServer)
        {
            openDoor.Value = !openDoor.Value;
        } else
        {
            OnInteractServerRpc(!openDoor.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnInteractServerRpc(bool value)
    {
        openDoor.Value = value;
    }

    private void OnDoorStateChange(bool oldValue, bool newValue)
    {
        //openDoor.Value = newValue;
        anim.SetBool("isOpen", newValue);
    }

    public override void OnNetworkSpawn()
    {
        openDoor.OnValueChanged += OnDoorStateChange;

        OnDoorStateChange(openDoor.Value, openDoor.Value);
    }

    public override void OnNetworkDespawn()
    {
        openDoor.OnValueChanged -= OnDoorStateChange;
    }
}
