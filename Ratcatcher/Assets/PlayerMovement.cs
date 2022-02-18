using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // reference to the Character Controller object
    public CharacterController controller;
    // movement speed
    public float speed = 3f;
    // gravity equal to earth -9.18 m/s^2
    public float gravity = -9.81f;
    // velocity, used for gravity to measure speed of character
    Vector3 velocity;

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

        // due to the way gravity works, time.deltatime needs squared, so 2 multiplications
        velocity.y += gravity * Time.deltaTime;     // only want to move on the y axis
        controller.Move(velocity * Time.deltaTime);
    }
}
