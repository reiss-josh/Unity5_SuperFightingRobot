using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float move;
    public float vMove;
    public float speed;
    public float jumpForce;
    public float groundRadius;
    public bool grounded;
    public bool facingEast;
    public MegaBullet megaBullet;

    public Transform GroundCheckL;
    public Transform GroundCheckR;
    public LayerMask groundLayers;

    private Rigidbody2D rb2d;
    private Animator animator;
    private bool keyJump;
    private bool keyShoot;
    private bool jumped;

    void Start()
    {
      //gather components
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
      //init self vars
        move = 0;
        speed = 6.75f;
        jumpForce = 850f;
        groundRadius = 0.2f;
        grounded = false;
        facingEast = true;
        jumped = false;
        keyJump = false;
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
        keyJump = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
        keyShoot = (Input.GetKeyDown(KeyCode.E));

        
        grounded = checkGrounded();
        perf_movement();
        if (keyShoot)
            {perf_shoot();}
        update_sprite();
    }

    void perf_movement()
    {
        rb2d.velocity = new Vector2(move * speed, rb2d.velocity.y);

        //jump
        if (grounded && keyJump)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            jumped = true;
        }
        
        if(jumped && (vMove <= 0))
        {
            jumped = false;
            if (rb2d.velocity.y > 0)
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }

    void perf_shoot()
    {
        MegaBullet bullet = Instantiate(megaBullet) as MegaBullet;
        if (facingEast)
        {
            bullet.transform.position = new Vector2(rb2d.position.x + 1, rb2d.position.y);
            bullet.shotDir = 2;
        }
        else
        {
            bullet.transform.position = new Vector2(rb2d.position.x - 1, rb2d.position.y);
            bullet.shotDir = 0;
        }
        //do the animation
    }

    //updates sprite
    void update_sprite()
    {
      //set sprite if moving
        if (move != 0)
            {animator.SetTrigger("is_walking");}
        else
            {animator.ResetTrigger("is_walking");}

      //set sprite if jumping
        if (!grounded)
            {animator.ResetTrigger("is_grounded");}
        else
            {animator.SetTrigger("is_grounded");}

      //set sprite if climbing
        /*
         * climbing sprite stuff
         */

      //flip sprite based on direction
        if (move > 0 && !facingEast)
            {Flip(); facingEast = true;}
        else if (move < 0 && facingEast)
            {Flip(); facingEast = false;}
    }

    //flips sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //checks for ground
    public bool checkGrounded()
    {
      //check left foot
        if (Physics2D.OverlapCircle(GroundCheckL.position, groundRadius, groundLayers))
            {return true;}
      //check right foot
        if (Physics2D.OverlapCircle(GroundCheckR.position, groundRadius, groundLayers))
            {return true;}
      //give up+
        return false;
    }
}