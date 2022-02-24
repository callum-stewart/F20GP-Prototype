using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : Interactable
{
    // Update is called once per frame
    void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Voice3");
            FindObjectOfType<GameManager>().hasKeyCard = true;
            Destroy(gameObject);
        }
    }
}
