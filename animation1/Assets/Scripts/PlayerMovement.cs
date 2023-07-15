using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private RaycastHit2D hit;

    ChangeCharacter changeChar;

    private float horizontalMovement;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float currentMoveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 30f;

    [Header("Variable Jump Modifier")]
    [SerializeField] private float gravityModifier = .5f;
    

    [Header("GroundCheck")]
    [SerializeField] private GameObject[] groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private bool isGrounded;

    [Header("Timer")]
    [SerializeField] private float coyoteTime;
    [SerializeField] private float currentCoyoteTime = .2f;
    [SerializeField] private float currentHangTime;
    [SerializeField] private float hangTime;

    [Header("WallCheck")]
    [SerializeField] private float raycastDistance = .5f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isTouchingWall;

    [Header("WallJump")]
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpForceX;
    [SerializeField] private float wallJumpForceY;
    [SerializeField] private float wallJumpCooldownTime;
    [SerializeField] private float currentWallJumpCoolDownTime;

    [Header("WallSlide")]
    [SerializeField] private float currentTimeBeforeWallSlide;
    [SerializeField] private float timeBeforeWallSlide;
    [SerializeField] private float wallSlideSpeed = .1f;

    private bool jump = false;
    private bool cutJump = false;
    private bool canWallJump = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        changeChar = GetComponent<ChangeCharacter>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) && currentCoyoteTime > 0)
        {
            jump = true;
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            cutJump = true;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 0;
            currentHangTime -= Time.deltaTime;
            if (currentHangTime < .1f)
            {
                rb.gravityScale = 5f;
            }
        }
        else
        {
            rb.gravityScale = 1f;
        }

        groundCollider = Physics2D.OverlapCircle(groundCheck[changeChar.currCharIndex].transform.position, groundCheckRadius, groundLayer);
        hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), raycastDistance, wallLayer);

        canWallJump = Input.GetKeyDown(KeyCode.Space) && isTouchingWall && !isGrounded;

        GroundCheck();
        WallCheck();
        Flip();
        HorizontalMove();
        WallJump();
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Jump();
            jump = false;
        }

        if (cutJump)
        {
            CutJump();
            cutJump = false;
        }
        WallSlide();
    }

    private void Flip()
    {
        if (horizontalMovement < 0)
        {
            transform.localScale = new(-1, 1);
        }
        else if (horizontalMovement > 0)
        {
            transform.localScale = new(1, 1);
        }
    }

    #region Movement Mechanic
    private void WallSlide()
    {
        if (isTouchingWall == true && isGrounded == false && isWallJumping == false)
        {
            currentMoveSpeed =0;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    private void WallJump()
    {
        
        if (canWallJump)
        {
            currentWallJumpCoolDownTime -= 
            currentMoveSpeed = moveSpeed;
            isWallJumping = true;
            currentTimeBeforeWallSlide = timeBeforeWallSlide;
            // Calculate the direction of the wall jump
            Vector2 wallJumpDirection = new Vector2(-transform.localScale.x, 1f).normalized;

            // Apply the jump force with the wall jump direction
            rb.velocity = new Vector2(-rb.velocity.x, 0f);

            rb.AddForce(new Vector2(wallJumpDirection.x * wallJumpForceX, wallJumpDirection.y * wallJumpForceY), ForceMode2D.Impulse);
        }
        else
        {
            canWallJump = false;
        }
        currentTimeBeforeWallSlide -= Time.deltaTime;
        if (currentTimeBeforeWallSlide <= 0)
        {
            isWallJumping = false;
        }
    }

    private void CutJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * gravityModifier);
    }
    #endregion

    #region Checks
    private void GroundCheck()
    {
        if (groundCollider != null)
        {
            isGrounded = true;
            currentCoyoteTime = coyoteTime;
            currentHangTime = hangTime;
            currentMoveSpeed = moveSpeed;
        }
        else
        {
            isGrounded = false;
            currentCoyoteTime -= Time.deltaTime;
        }
    }
    private void WallCheck()
    {
        if (hit.collider != null)
        {
            isTouchingWall = true;
            currentWallJumpCoolDownTime = wallJumpCooldownTime;
        }
        else
        {
            isTouchingWall = false;
        }
    }
    #endregion

    #region Basic Movement
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HorizontalMove()
    {
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck[0].transform.position, groundCheckRadius);
        Gizmos.DrawRay(transform.position, Vector2.right * raycastDistance);
    }
}
