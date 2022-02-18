using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // reference to the Character Controller object
    public CharacterController controller;
    // movement speed
    public float speed = 3f;

    // Update is called once per frame
    void Update()
    {
        // get the players current position
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // takes the position the player is facing, creates a vector pointing in desired direction
        Vector3 move = transform.right * x + transform.forward * z;

        // move 
        controller.Move(move * speed * Time.deltaTime);
    }
}
