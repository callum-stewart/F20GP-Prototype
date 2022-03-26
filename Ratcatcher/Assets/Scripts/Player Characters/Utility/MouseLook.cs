using UnityEngine;
using Mirror;

public class MouseLook : MonoBehaviour
{
    // sensitivity to control the speed of the mouse
    public float mouseSensitivity = 10f;

    // holder for the player body 
    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // only enable the camera for the clients player object
        if (transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
            transform.GetChild(0).gameObject.SetActive(true);
        // prevent mouse movement
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // only allow client to control their own player object
        if (transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
        {
            // get the position of the mouse in the scene
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;    //Time.deltaTime to remain constant with frame rate
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // -= as += is the mirror rotation
            xRotation -= mouseY;
            // clamp to prevent going too far in roation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate the camera (y axis movement)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // rotate the body (x axis movement)
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
