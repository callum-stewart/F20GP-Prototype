using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    [Command]
    public void CmdLightChange(LightSwitch light)
    {
        light.RpcChangeLights();
    }
}
