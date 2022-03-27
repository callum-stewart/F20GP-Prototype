using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    public LightSwitch light;

    [Command]
    public void CmdLightChange()
    {
        light.RpcChangeLights();
    }

    private void Update()
    {
        if (light != null)
        {
            if (light.interactive && Input.GetButtonDown("Interact"))
            {
                FindObjectOfType<AudioManager>().Play("Flashlight");

                if (light.powered)
                    CmdLightChange();
                //changeLights();
            }
        }
    }
}
