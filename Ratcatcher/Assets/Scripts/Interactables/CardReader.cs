using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : Interactable
{
    public PoweredDoor door;

    // Update is called once per frame
    void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            if(FindObjectOfType<GameManager>().hasKeyCard)
                door.doorInteraction();
            else
                FindObjectOfType<UserInterface>().setInfo("You don't have a key card", 10f);
        }
    }
}
