using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class BasementDoors : NetworkBehaviour
{
    private Animator anim;
    private NetworkVariable<bool> doorOpen = new(false);

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public bool GetDoorsState()
    {
        return doorOpen.Value;
    }

    public void ChangeDoorStateForServer(bool value)
    {
        doorOpen.Value = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnInteractOpenDoorsServerRpc(bool value)
    {
        doorOpen.Value = value;
    }

    private void OnDoorOpenChange(bool oldValue,  bool newValue)
    {
        anim.SetBool("isEntranceOpen", newValue);
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
