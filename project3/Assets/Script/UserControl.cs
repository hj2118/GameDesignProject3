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
    }

    IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        speed = intialSpeed;
        dashing = false;
    }
}
