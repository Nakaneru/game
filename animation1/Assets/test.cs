using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float gravityModifier = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float currentHangTime;
    [SerializeField] private float hangTime;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private bool isGrounded;
    private bool jump = false;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float currentCoyoteTime = .2f;
    [SerializeField] private float raycastDistance = .5f;
    private RaycastHit2D hit;
    [SerializeField] private bool isWallSliding;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private float wallSlideSpeed = .1f;
    private bool canWallJump = false;
    [SerializeField] private float wallJumpForceX;
    [SerializeField] private float wallJumpForceY;
    [SerializeField] private float currentTimeBeforeWallSlide;
    [SerializeField] private float timeBeforeWallSlide;

    private float horizontalMovement;

    [Header("Horizontal Movement")]
    [Range(0, 1f)]
    [SerializeField] private float horizontalDamping = 0f;
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        float horizontalVelocity = rb.velocity.x;
        horizontalVelocity += horizontalMovement * moveSpeed;
        horizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        Debug.Log(horizontalVelocity);
        //rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        if (!isWallSliding)
        {
            rb.velocity = new Vector2(horizontalVelocity * moveSpeed, rb.velocity.y);
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && currentCoyoteTime>0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if(currentCoyoteTime<.01f)
                Debug.Log("aaa");
            jump = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * gravityModifier);
            jump = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 0;
            currentHangTime -= Time.deltaTime;
            if (currentHangTime < .1f )
            {
                rb.gravityScale = 5f;
            }
        }
        else
        {
            rb.gravityScale = 1f;
        }

        groundCollider = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundLayer);

        if (groundCollider != null)
        {
            isGrounded = true;
            currentCoyoteTime = coyoteTime;
            jumpForce = 10;
            currentHangTime = hangTime;
        }
        else
        {
            isGrounded = false;
            currentCoyoteTime -= Time.deltaTime;
        }

        hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x,0), raycastDistance, wallLayer);
        if(hit.collider != null)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall =  false;
        }

        if (isTouchingWall == true && isGrounded == false && isWallJumping == false)
        {
            isWallSliding = true; 
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector2(-1,1);
        }
        else if (Input.GetAxis("Horizontal")>0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        canWallJump = Input.GetKeyDown(KeyCode.Space) && isTouchingWall && !isGrounded;

        if (canWallJump)
        {
            isWallJumping = true;
            currentTimeBeforeWallSlide = timeBeforeWallSlide;
            if (isTouchingWall && !isGrounded && Input.GetAxis("Horizontal") != transform.localScale.x)
            {
                // Calculate the direction of the wall jump
                Vector2 wallJumpDirection = new Vector2(-transform.localScale.x, 1f).normalized;

                // Apply the jump force with the wall jump direction
                rb.velocity = new Vector2(rb.velocity.x, 0f); // Clear vertical velocity

                rb.AddForce(new Vector2(wallJumpDirection.x * wallJumpForceX, wallJumpDirection.y * wallJumpForceY), ForceMode2D.Impulse);

                // Disable wall jumping temporarily
                canWallJump = false;
            }

        }
        currentTimeBeforeWallSlide -= Time.deltaTime;
        if (currentTimeBeforeWallSlide <= 0)
        {
            isWallJumping = false;
        }

    }

    private void FixedUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        Gizmos.DrawRay(transform.position, Vector2.right * raycastDistance);
    }
}

