using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public Light flashLight;
    public Camera playerView;

    public float onIntensity = 5f;
    public float offIntensity = 0f;
    public float range = 10f;

    public AudioSource clickSound;

    bool flashOn = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
            setFlash();
    }

    void setFlash()
    {
        flashOn = !flashOn;
        // set intensity dependant on if flash is on or not
        flashLight.intensity = (flashOn ? onIntensity : offIntensity);
        // play the click sound
        FindObjectOfType<AudioManager>().Play("Flashlight");

        // check for hits with raycast
        if (flashOn)
            stun();
    }

    void stun()
    {
        // create a raycast object originating from the player, moving in the direction they are facing
        RaycastHit hit;
        if (Physics.Raycast(playerView.transform.position, playerView.transform.forward, out hit, range))
        {
            Debug.Log(hit.collider.name);
            // if the hit component is the rat catcher, call stunHit
            RatCatcher ratCatcher = hit.transform.GetComponent<RatCatcher>();
            if (ratCatcher != null)
            {                
                ratCatcher.stunHit();
            }
                

        }
    }
}

