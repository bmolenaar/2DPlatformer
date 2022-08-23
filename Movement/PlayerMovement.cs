using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Reference to the CharacterController2D script containing Move function
    public CharacterController2D controller;
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D col;

    //Player movement speed
    public float runSpeed = 40f;

    //Determine input
    float horizontalMove;
    bool canJump = false;
    bool canCrouch = false;

    //Determine what is ground
    public LayerMask groundLayer;

    public float jumpBufferLength = 0.1f;
    float jumpBufferCount;

    public ParticleSystem footstepParticles;
    ParticleSystem.EmissionModule footEmission;

    public ParticleSystem impactParticle;
    bool wasGrounded;

    bool flipflop;
    public AudioClip[] footstepSounds;
    AudioSource audioSource;

    
    public AudioClip landingSound;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        footEmission = footstepParticles.emission;
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        //Left arrow key or "S" key = -1, Right arrow or "D" = 1
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //Give the player some leway incase they press jump slightly before they hit the ground
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferLength;
            
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        if (jumpBufferCount >= 0)
        {
            canJump = true;
            jumpBufferCount = 0;
        }

        //Allow player to control jump height depending on how long button is held
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        //if (Input.GetButtonDown("Crouch"))
        //{
        //    canCrouch = true;
        //}
        //else if(Input.GetButtonUp("Crouch"))
        //{
        //    canCrouch = false;
        //}

        //Handle dust particles while running
        if (IsGrounded() && Input.GetAxisRaw("Horizontal") != 0)
        {
            footEmission.rateOverTime = 30f;
        }
        else
        {
            footEmission.rateOverTime = 0;
        }

        //Handle impact particles when player lands after jump
        //If I was in the air on the prev frame, but am now on the ground, spawn particle
        if (!wasGrounded && IsGrounded())
        {
            impactParticle.gameObject.SetActive(true);
            impactParticle.Stop();
            impactParticle.transform.position = footstepParticles.transform.position;
            impactParticle.Play();
            AudioSource.PlayClipAtPoint(landingSound, transform.position, 1);
        }

        //Keep track of if I was in the air on the previous frame
        wasGrounded = IsGrounded();

    }

    private void FixedUpdate()
    {
        //Move the character depending on input and multiply by frame rate
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, canJump);
        canJump = false;

        if (horizontalMove != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (!IsGrounded())
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        //if (IsGrounded())
        //{
        //    canJump = false;
        //}
        
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.1f;

        //RaycastHit2D hit = Physics2D.Raycast(col.bounds.center, direction, distance, groundLayer);
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, direction, distance, groundLayer);
        Debug.DrawRay(col.bounds.center + new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + distance), Color.red);
        Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + distance), Color.red);
        Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, col.bounds.extents.y), Vector2.right * (col.bounds.extents.x));

        //Debug.DrawRay(col.bounds.center, direction, Color.green);


        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    //Called from animation event within Walk anim
    private void FootstepSound()
    {
        flipflop = !flipflop;

        if (IsGrounded() && Input.GetAxisRaw("Horizontal") != 0)
        {
            if (flipflop)
            {
                audioSource.clip = footstepSounds[0];
                audioSource.Play();
            }
            else
            {
                audioSource.clip = footstepSounds[1];
                audioSource.Play();
            }
        }

    }
}
