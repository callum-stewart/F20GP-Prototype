using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string displayText;

    // display the control text
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo(displayText, 10f);
    }

    // interaction
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact"))
            FindObjectOfType<AudioManager>().Play("Voice1");
    }

    // close text
    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo("");
    }
}
