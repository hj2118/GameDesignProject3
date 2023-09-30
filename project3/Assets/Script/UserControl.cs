using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public Transform spawnPoint;

    public int speed = 3;
    public float xDirection;
    
    public int jumpVal = 20;
    public int jumps = 1;
    public int maxJumps = 1;
    public bool grounded = false;
    public Transform playerBottom;
    public LayerMask platform;
    public bool jump = true;

    GameManager _gameManager;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        publicvar.playerDead = false;       // TODO: for game loop

        transform.position = spawnPoint.transform.position;         // initial position
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

        // jump
        //grounded = Physics2D.OverlapCircle(playerBottom.position, .2f, platform);
        //print(grounded);
   
        //if (grounded && Input.GetButton("Jump"))
        //{
        //    print("Jump");
        //    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        //    _rigidbody.AddForce(new Vector2(0, jumpVal));
        //}

        
    }

    void Update()
    {
        if (_rigidbody.position.y < -6)
        {
            //transform.position = new Vector2(0, 0); 
            transform.position = spawnPoint.transform.position;
        }

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
    }
}
