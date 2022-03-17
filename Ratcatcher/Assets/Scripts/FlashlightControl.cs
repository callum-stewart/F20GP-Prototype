using UnityEngine;
using Mirror;

public class FlashlightControl : NetworkBehaviour
{
    [SerializeField]
    public Light flashLight;
    public Camera playerView;

    public float onIntensity = 5f;
    public float offIntensity = 0f;
    public float range = 10f;

    public AudioSource clickSound;

    [SyncVar]
    bool flashOn = true;

    // Update is called once per frame
    void Update()
    {
        // only allow client to control their own torch
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            // when button is clicked send command to server to turn on/off the flashlight
            if (Input.GetButtonDown("Fire2"))
                CmdSetFlash();
        }
    }

    [Command]
    void CmdSetFlash()
    {
        // check for hits with raycast
        if (flashOn)
            stun();
        // Send command to clients to update the flashlights
        RpcFlash();
    }

    [ClientRpc]
    void RpcFlash()
    {
        flashOn = !flashOn;
        // set intensity dependant on if flash is on or not
        flashLight.intensity = (flashOn ? onIntensity : offIntensity);
        // play the click sound
        FindObjectOfType<AudioManager>().Play("Flashlight");
    }

    void stun()
    {
        // create a raycast object originating from the player, moving in the direction they are facing
        RaycastHit hit;
        if (Physics.Raycast(playerView.transform.position, playerView.transform.forward, out hit, range))
        {
            // if the hit component is the rat catcher, call stunHit
            RatCatcher ratCatcher = hit.transform.GetComponent<RatCatcher>();
            if (ratCatcher != null)        
                ratCatcher.stunHit();
        }
    }
}

