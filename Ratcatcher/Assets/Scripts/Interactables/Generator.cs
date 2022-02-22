using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactable
{
    private void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Voice2");
        }
    }
}
