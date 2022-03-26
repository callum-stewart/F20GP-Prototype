using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabsStartScript : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<GameManager>().theLabs();
    }
}
