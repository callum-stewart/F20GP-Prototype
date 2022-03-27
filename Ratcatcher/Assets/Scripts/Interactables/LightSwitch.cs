using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LightSwitch : Interactable
{
    [SerializeField]
    public Light[] circuit;
    [SyncVar]
    public bool powered = false;
    [SyncVar]
    public bool isOn = true;

    // Update is called once per frame
    void Update()
    {
        if (interactive && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<AudioManager>().Play("Flashlight");

            if (powered)
                playerCollider.gameObject.GetComponent<PlayerInteraction>().CmdLightChange(this);
                //changeLights();
        }
    }

    [Command]
    public void CmdChangeLights()
    {
        RpcChangeLights();
    }

    [ClientRpc]
    public void RpcChangeLights()
    {
        isOn = !isOn;

        // turn all the lights on or off
        foreach (Light l in circuit)
            l.intensity = isOn ? l.intensity = 1 : l.intensity = 0;
    }

    // switch on or off lights in circuit
    void changeLights()
    {
        isOn = !isOn;

        // turn all the lights on or off
        foreach (Light l in circuit)
            l.intensity = isOn ? l.intensity = 1 : l.intensity = 0;
    }

    // generator has been clicked
    public void generatorSwitch()
    {
        powered = !powered;
        // if off, set isOn to true so it is switched in changeLights();
        if (!powered)
            isOn = true;
        changeLights();
    }
}
