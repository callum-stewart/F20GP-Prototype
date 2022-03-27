using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredDoor : MonoBehaviour
{
    public GameObject doorLeft;
    public GameObject doorRight;
    public bool zPerpendicular;
    public bool startClosed;
    
    public bool closed = false;
    bool closing = false;
    Vector3 openDistance;
    int doorMovements = 100;
    int closeProgress;

    private void Awake()
    {
        closeProgress = doorMovements;
        openDistance = zPerpendicular ? new Vector3(.75f, 0f, 0f) : new Vector3(0f, 0f, -.75f);

        if (startClosed)
            doorInteraction();  
    }

    private void Update()
    {
        if (closing)
        {
            Vector3 closeInc = openDistance / doorMovements;
            doorLeft.transform.position += closed ? -closeInc : closeInc;
            doorRight.transform.position += closed ? closeInc : -closeInc;
            closeProgress -= 1;

            if (closeProgress == 0)
            {
                closing = false;
                closeProgress = doorMovements;
                Debug.Log("closed: " + closed);
            }   
        }
    }

    public void doorInteraction()
    {
        closed = !closed;
        closing = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if(closed && !closing && Input.GetButtonDown("Interact"))
        {
            Debug.Log("hello");
            doorInteraction();
        }
    }
}
