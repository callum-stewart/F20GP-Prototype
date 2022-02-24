using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string displayText;
    public string disableText = "";
    protected bool interactive = false;
    protected bool disabled = false;

    // display the control text
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!disabled)
        {
            FindObjectOfType<UserInterface>().setInfo(displayText, 10f);
            interactive = true;
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
