using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseLever : Interactable
{
    private void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Voice1");
            FindObjectOfType<RatCatcher>().Activate();
        }
    }
}
