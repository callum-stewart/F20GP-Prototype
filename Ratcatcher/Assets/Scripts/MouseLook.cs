using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // sensitivity to control the speed of the mouse
    public float mouseSensitivity = 100f;

    // holder for the player body 
    public Transform playerBody;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get the position of the mouse in the scene
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;    //Time.deltaTime to remain constant with frame rate
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
