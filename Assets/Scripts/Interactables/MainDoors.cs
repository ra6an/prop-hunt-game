using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoors : Interactable
{
    [SerializeField]
    private bool doorOpen;

    protected override void Interact() 
    {
        doorOpen = !doorOpen;
        gameObject.GetComponent<Animator>().SetBool("isOpen", doorOpen);
    }
}
