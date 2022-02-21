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
        clickSound.Play();

        // check for hits with raycast
        if (flashOn)
            stun();
    }

    void stun()
    {
        Debug.Log("attempt Stun");
        // create a raycast object originating from the player, moving in the direction they are facing
        RaycastHit hit;
        if (Physics.Raycast(playerView.transform.position, playerView.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            // if the hit component is the rat catcher, call stunHit
            RatCatcherController ratCatcher = hit.transform.GetComponent<RatCatcherController>();
            if (ratCatcher != null)
            {                
                ratCatcher.stunHit();
            }
                

        }
    }
}

