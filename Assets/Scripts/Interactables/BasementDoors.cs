using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BasementDoors : Interactable
{
    [SerializeField]
    private GameObject basementEntrance;
    private bool doorOpen;
    protected override void Interact()
    {
        Debug.Log("Otvori/Zatvori");
        doorOpen = !doorOpen;
        basementEntrance.GetComponent<Animator>().SetBool("isEntranceOpen", doorOpen);
    }
}
