using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    public Light[] circuit;
    bool powered;
    bool isOn;

    private void Awake()
    {
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Flashlight");
            changeLights();
        }
    }

    // switch on or off lights in circuit
    void changeLights()
    {
        foreach(Light l in circuit)
        {
            if (isOn)
                l.intensity = 0;
            else
                l.intensity = 1;

        }

        isOn = !isOn;
    }
}
