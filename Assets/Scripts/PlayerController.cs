using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float move, vMove, speed;
    public float jumpForce;
    public float groundRadius;
    public bool grounded;
    public bool facingEast;
    public float shotCooldownShort, shotCooldownMax;
    public float shotCountCooldownTop;

    public MegaBullet megaBullet;
    public Transform GroundCheckL, GroundCheckR;
    public LayerMask groundLayers;
    private Rigidbody2D rb2d;
    private Animator animator;
    private AudioSource megaBusterSound;

    private bool keyJump, keyShoot;
    private bool jumped;
    private bool isShot, didShotMax;

    private int shotCount;
    private float shotCooldown, shotCountCooldown;
    
    void Start()
    {
      //gather components
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        megaBusterSound = GetComponent<AudioSource>();
      //init self vars
        speed = 6.75f;
        jumpForce = 850f;
        groundRadius = 0.2f;
        grounded = false;
        facingEast = true;
        shotCooldownShort = 0.15f;
        shotCooldownMax = 0.5f;
        shotCountCooldownTop = 0.3f;

        shotCooldown = 0.5f;
        shotCountCooldown = 0.5f;

        jumped = false;
        keyJump = false;
        isShot = false;
        didShotMax = false;
        shotCount = 0;
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
        keyJump = Input.GetButtonDown("Jump");
        keyShoot = Input.GetButtonDown("Fire1");
        

        grounded = checkGrounded();
        perf_movement();
        perf_shoot();
        update_sprite();
    }

    void perf_shoot()
    {
        if (keyShoot || isShot)
        {
            if (shotCooldown <= 0.0f)
            {
                megaBusterSound.Play(0);
              //direction
                MegaBullet bullet = Instantiate(megaBullet) as MegaBullet;
                if (facingEast)
                {
                    bullet.transform.position = new Vector2(rb2d.position.x + 1.5f, rb2d.position.y + 0.1f);
                    bullet.shotDir = 2;
                }
                else
                {
                    bullet.transform.position = new Vector2(rb2d.position.x - 1.5f, rb2d.position.y + 0.1f);
                    bullet.shotDir = 0;
                }
            //timing
                didShotMax = false;
                if ((isShot == false) && (shotCountCooldown == 0.0f))
                { shotCount = 0; }
                else
                {
                    isShot = false;
                    shotCount++;
                }
                if (shotCount >= 3)
                {
                    shotCooldown = shotCooldownMax;
                    shotCount = 0;
                    didShotMax = true;
                }
                else
                    shotCooldown = shotCooldownShort;
                shotCountCooldown = shotCountCooldownTop;
            }
            else if (!(didShotMax))
                isShot = true;
        }
        //more timing
        if (shotCooldown > 0.0f)
            { shotCooldown -= Time.deltaTime; }
        if (shotCountCooldown > 0.0f)
            { shotCountCooldown -= Time.deltaTime; }
        else
            if (!isShot)
            shotCount = 0;
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

        if (shotCooldown >= 0.0f)
            { animator.SetTrigger("is_shooting"); }
        else
            { animator.ResetTrigger("is_shooting"); }

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
      //give up
        return false;
    }
}
