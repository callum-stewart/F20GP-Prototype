using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Interactable : NetworkBehaviour
{
    public string displayText;
    public string disableText = "";
    protected bool interactive = false;
    protected bool disabled = false;
    protected Collider playerCollider;

    // display the control text
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!disabled)
        {
            FindObjectOfType<UserInterface>().setInfo(displayText, 10f);
            interactive = true;
            playerCollider = other;
        }
        else
            FindObjectOfType<UserInterface>().setInfo(disableText, 10f);
        
    }

    // close text
    public virtual void OnTriggerExit(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo("");
        interactive = false;
    }
}
