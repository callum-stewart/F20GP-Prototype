using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public Light flashLight;
    public float onIntensity = 5f;
    public float offIntensity = 0f;

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
    }
}

