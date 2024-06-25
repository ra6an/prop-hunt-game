using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDoubleDoors : Interactable
{
    private BasementDoors basementDoors;

    private void Awake()
    {
        basementDoors = transform.GetComponentInParent<BasementDoors>();
    }

    protected override void Interact()
    {
        bool value = basementDoors.GetDoorsState();
        if(IsServer)
        {
            basementDoors.ChangeDoorStateForServer(!value);
        } else
        {
            basementDoors.OnInteractOpenDoorsServerRpc(!value);
        }
    }
}
