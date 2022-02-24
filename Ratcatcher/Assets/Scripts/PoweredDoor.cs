using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredDoor : MonoBehaviour
{
    public GameObject doorLeft;
    public GameObject doorRight;
    bool isOpen = true;
    bool closing = false;
    Vector3 openDistance = new Vector3(.75f, 0f, 0f);
    int doorMovements = 100;
    int closeProgress;

    private void Awake()
    {
        closeProgress = doorMovements;
    }

    private void Update()
    {
        if (closing)
        {
            Vector3 closeInc = openDistance / doorMovements;
            doorLeft.transform.position += isOpen ? closeInc : -closeInc;
            doorRight.transform.position += isOpen ? -closeInc : closeInc;
            closeProgress -= 1;

            if (closeProgress == 0)
                closing = false;
        }

    }

    public void doorInteraction()
    {
        isOpen = !isOpen;
        closing = true;
    }
}
