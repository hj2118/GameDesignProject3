using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public Transform spawnPoint;

    public float intialSpeed = 3f;
    public float speed = 3f;
    public float xDirection;

    // Wall Jump
    public float wallJumpForce = 10f;
    public LayerMask wallLayer;
    public float wallJumpTime = 0.2f;
    private float wallJumpTimeCounter;
    private bool isWallJumping = false;

    // jump
    public int jumpVal = 12;
    public int jumps = 1;
    public int maxJumps = 1;
    public bool grounded = false;
    public Transform playerBottom;
    public LayerMask platform;
    public bool jump = true;

    // dash
    public float dashSpeed = 8f;
    public float dashDuration = 2f;
    bool dashing;

    GameManager _gameManager;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        publicvar.playerDead = false;       // TODO: for game loop

        transform.position = spawnPoint.transform.position;         // initial position

        // for dash
        dashing = false;
    }

    void FixedUpdate()
    {
        if (publicvar.playerDead)
        {
            return;
        }

        // horizontal move
        float horizontalMovement = Input.GetAxis("Horizontal") * speed;
        _rigidbody.velocity = new Vector2(horizontalMovement, _rigidbody.velocity.y);

        // TODO: once we have a sprite for player
        //xDirection = transform.localScale.x;

        //if (horizontalMovement < 0 && xDirection > 0 || horizontalMovement > 0 && xDirection < 1)
        //{
        //    transform.localScale *= new Vector2(-1, 1);
        //}
        
        // dash
        if (dashing)
        {
            StartCoroutine(Dash());
        }
    }

    void Update()
    {
        // respawn
        if (_rigidbody.position.y < -6)
        {
            //transform.position = new Vector2(0, 0); 
            transform.position = spawnPoint.transform.position;
        }

        // jump
        grounded = Physics2D.OverlapCircle(playerBottom.position, .2f, platform);

        if (grounded)
        {
            jumps = maxJumps;
        }

        if (grounded && Input.GetKey(KeyCode.Space))
        {
            _rigidbody.velocity = Vector2.up * jumpVal;
        }

        else if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            _rigidbody.velocity = Vector2.up * jumpVal;
            jumps--;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.Z))
        {
            dashing = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !grounded && IsTouchingWall())
        {
            isWallJumping = true;
            wallJumpTimeCounter = wallJumpTime;
            _rigidbody.velocity = new Vector2(-xDirection * wallJumpForce, jumpVal);
        }

        if (isWallJumping && wallJumpTimeCounter > 0)
        {
            // Continue applying wall jump force for a short time
            _rigidbody.velocity = new Vector2(-xDirection * wallJumpForce, _rigidbody.velocity.y);
            wallJumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isWallJumping = false;
        }

    }

    private bool IsTouchingWall()
    {
        float wallCheckDistance = 0.2f; // Adjust this as needed

        // Cast rays to check if the player is touching a wall
        RaycastHit2D hit = Physics2D.Raycast(playerBottom.position, Vector2.right * xDirection, wallCheckDistance, wallLayer);

        return hit.collider != null;
    }

    IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        speed = intialSpeed;
        dashing = false;
    }
}
