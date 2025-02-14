using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public float playerHeight = 2f;
    private Vector3 velocity;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private Rigidbody rb;
    public bool readyToJump;
    public float groundDrag;

<<<<<<< Updated upstream
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

	public bool isP1 = true;
	public bool isP2 = false;
	public bool isP3 = false;
	public bool isP4 = false;


    private void Start()
=======
    void Start()
>>>>>>> Stashed changes
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    void Update()
    {
        // Movement logic
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 direct = new Vector3(horiz, 0f, vert).normalized;

        if (direct.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);  // Prevent stopping vertical motion
        }

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

<<<<<<< Updated upstream
    private void MyInput()
    {

		if (isP1){
			horizontalInput = Input.GetAxisRaw("P1_Horizontal");
        	verticalInput = Input.GetAxisRaw("P1_Vertical");
		}
		else if (isP2){
			horizontalInput = Input.GetAxisRaw("P2_Horizontal");
        	verticalInput = Input.GetAxisRaw("P2_Vertical");
		}
		else if (isP3){
			horizontalInput = Input.GetAxisRaw("P3_Horizontal");
        	verticalInput = Input.GetAxisRaw("P3_Vertical");
		}
		else if (isP4){
			horizontalInput = Input.GetAxisRaw("P4_Horizontal");
        	verticalInput = Input.GetAxisRaw("P4_Vertical");
		}
		else {
			Debug.Log("no player assigned for inputs");
		}



        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
=======
        // Jump
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
>>>>>>> Stashed changes
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), 0.1f);  // Ensure jump is reset after a short delay
        }

        // Drag adjustment
        rb.drag = isGrounded ? groundDrag : 0;
        BetterFall();
    }

    private void BetterFall()
    {
        if (rb.velocity.y < 0)  // When falling
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))  // When ascending but not holding jump
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Reset vertical velocity
        float jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Apply proper jump force
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
