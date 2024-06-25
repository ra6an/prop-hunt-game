using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainDoors : Interactable
{
    private Animator anim;
    private NetworkVariable<bool> doorOpen = new(false);

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void Interact() 
    {
        if(IsServer)
        {
            doorOpen.Value = !doorOpen.Value;
        } else
        {
            OnDoorInteractServerRpc(!doorOpen.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnDoorInteractServerRpc(bool value)
    {
        doorOpen.Value = value;
    }

    private void OnDoorOpenChange(bool oldValue, bool newValue)
    {
        anim.SetBool("isOpen", newValue);
    }

    public override void OnNetworkSpawn()
    {
        doorOpen.OnValueChanged += OnDoorOpenChange;

        OnDoorOpenChange(doorOpen.Value, doorOpen.Value);
    }

    public override void OnNetworkDespawn()
    {
        doorOpen.OnValueChanged -= OnDoorOpenChange;
    }
}
