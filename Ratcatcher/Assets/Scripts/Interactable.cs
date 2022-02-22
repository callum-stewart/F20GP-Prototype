using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string displayText;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo(displayText, 10f);
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<UserInterface>().setInfo("");
    }
}
