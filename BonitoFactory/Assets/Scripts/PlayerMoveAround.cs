using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAround : MonoBehaviour
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

    void Start()
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

        // Jump
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
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
