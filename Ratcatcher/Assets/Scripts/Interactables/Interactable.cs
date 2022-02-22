using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string displayText;
    protected bool interactive = false;

    // display the control text
    public virtual void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo(displayText, 10f);
        interactive = true;
    }

    // close text
    public virtual void OnTriggerExit(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo("");
        interactive = false;
    }
}
