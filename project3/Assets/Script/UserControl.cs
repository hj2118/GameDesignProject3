using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool isTouchingWall = false;
    public Transform playerRight;
    public Transform playerLeft;

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
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _rigidbody = GetComponent<Rigidbody2D>();

        publicvar.playerDead = false;
        spawnPoint.transform.position = new Vector2(0,0);
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

        xDirection = transform.localScale.x;

        if (horizontalMovement < 0 && xDirection > 0 || horizontalMovement > 0 && xDirection < 0)
        {
            transform.localScale *= new Vector2(-1, 1);
        }

        // dash
        if (dashing)
        {
            StartCoroutine(Dash());
        }

        checkTouchingWall();
    }

    void Update()
    {
        // respawn
        if (_rigidbody.position.y < -6)
        {
            publicvar.lives--;

            if (publicvar.lives > 0) {
                //transform.position = new Vector2(0, 0); 
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                spawnPoint.transform.position = new Vector2(0, 0);
                publicvar.playerDead = true;
            }
        }

        // jump & double jump
        grounded = Physics2D.OverlapCircle(playerBottom.position, .2f, platform);

        if (grounded)
        {
            jumps = maxJumps;
        }

        if (grounded && Input.GetKey(KeyCode.Space))
        {
            _rigidbody.velocity = Vector2.up * jumpVal;
        }

        else if (Input.GetKeyDown(KeyCode.Space) && (jumps > 0) && !isTouchingWall)
        {
            _rigidbody.velocity = Vector2.up * jumpVal;
            jumps--;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.Z))
        {
            dashing = true;
        }

        // wall jump
        if (Input.GetKeyDown(KeyCode.Space) && !grounded && isTouchingWall)
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

    public void checkTouchingWall()
    {
        float wallCheckDistance = 0.2f; // Adjust this as needed

        // Cast rays to check if the player is touching a wall
        //RaycastHit2D hit1 = Physics2D.Raycast(playerRight.position, Vector2.right * xDirection, wallCheckDistance, wallLayer);
        //RaycastHit2D hit2 = Physics2D.Raycast(playerLeft.position, Vector2.right * xDirection, wallCheckDistance, wallLayer);
        
        bool hit1 = Physics2D.OverlapCircle(playerRight.position, wallCheckDistance, wallLayer);
        bool hit2 = Physics2D.OverlapCircle(playerLeft.position, wallCheckDistance, wallLayer);

        //if ((hit1.collider != null) || (hit2.collider != null))
        if (hit1 || hit2)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
        //return hit.collider != null;
    }

    IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        speed = intialSpeed;
        dashing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            //if (SceneManager.GetActiveScene().name == "GameScene")
            //{
            //    _gameManager.checkBestTime();
            //}

            publicvar.complete = true;
        }

        else if (other.CompareTag("CheckPoint"))
        {
            spawnPoint.transform.position = other.transform.position;
            Destroy(other);
        }
    }
}
