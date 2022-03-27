using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Generator : Interactable
{
    List<LightSwitch> lights = new List<LightSwitch>();

    private void Awake()
    {
        foreach (LightSwitch l in FindObjectsOfType<LightSwitch>())
        {
            l.generatorSwitch();
            lights.Add(l);
        }
    }

    private void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Voice2");
            generatorClicked();
        }
    }

    // called by release lever to break the generator
    public void generatorClicked()
    {
        foreach (LightSwitch l in lights)
            l.generatorSwitch();
    }
}
