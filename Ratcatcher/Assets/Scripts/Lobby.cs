using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    private void Awake()
    {
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
    }
}
