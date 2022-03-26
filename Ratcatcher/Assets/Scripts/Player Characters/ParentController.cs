using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    
    public CharacterController controller;  // reference to the Character Controller object
    public string playerName;

    public float speed = 2.5f;    // movement speed
    public float sprintSpeed = 6f;
    public float gravity = -12f;  // gravity equal to earth -9.18 m/s^2
    public float jumpHeight = 1f;

    public Transform groundCheck;   // reference to the ground checking object
    public float groundDistance = 0.4f; // the radius of the sphere that performs the ground check
    public LayerMask groundMask;    // used to check for different things that might need checked
    
    Vector3 velocity;   // velocity, used for gravity to measure speed of character
    bool isGrounded;
    bool isSprinting;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneChange;
    }

    private void SceneChange(Scene scene, LoadSceneMode mode)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {
         SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        // only allow client to control their own player object
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            performGroundCheck();
            movePlayer();

            // only jump if player is grounded
            if (Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // only sprint if player is grounded and sprint isnt on cooldown
            if (Input.GetButton("Sprint") && isGrounded)
                isSprinting = true;
            else
                isSprinting = false;

            // due to the way gravity works, time.deltatime needs squared, so 2 multiplications
            velocity.y += gravity * Time.deltaTime;     // only want to move on the y axis
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void movePlayer()
    {
        // get the players current position
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // takes the position the player is facing, creates a vector pointing in desired direction
        Vector3 move = transform.right * x + transform.forward * z;

        // move 
        controller.Move(move * (isSprinting ? sprintSpeed : speed) * Time.deltaTime);
    }

    void performGroundCheck()
    {
        // creates invisible sphere at groundCheck's position, returns true if anything in groundMask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // force player onto ground
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;   // 0f caused weird errors with not properly returning the surface, leaving a gap
    }

}
