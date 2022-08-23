using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;

    public float moveSpeed = 3f;
    public float horizontalMove;
    public float verticalMove;
    public float moveDistance;
    public float maxHealth = 1f;
    public float curHealth;
    public float deathThrust = 10f;
    public float damage = 0.5f;

    bool m_FacingRight = false;
    public bool isDead = false;

    public GameObject killVolume;

    bool playDeathSound;
    public AudioClip[] deathSounds;

    Vector2 startPos;
    float res_hMove;
    float res_vMove;
    float res_grav;
    float res_mass;

    private void Start()
    {
        startPos = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        horizontalMove *= moveSpeed;
        verticalMove *= moveSpeed;
        curHealth = maxHealth;

        res_hMove = horizontalMove;
        res_vMove = verticalMove;
        res_grav = rb.gravityScale;
        res_mass = rb.mass;
    }

    void Update()
    {

        if (curHealth <= 0)
        {
            Die();
        }

        if (transform.position.y <= killVolume.transform.position.y + 10)
        {
            anim.SetBool("IsDead", false);           
        }

        if (transform.position.y <= killVolume.transform.position.y)
        {
            gameObject.SetActive(false);
        }

        if (this.transform.position.x < startPos.x - moveDistance && !isDead)
        {
            horizontalMove = 1 * moveSpeed;
        }
        else if (this.transform.position.x > startPos.x + moveDistance && !isDead)
        {
            horizontalMove = -1 * moveSpeed;
        }

        if (this.transform.position.y < startPos.y - moveDistance && !isDead)
        {
            verticalMove = 1 * moveSpeed;
        }
        else if (this.transform.position.y > startPos.y + moveDistance && !isDead)
        {
            verticalMove = -1 * moveSpeed;
        }

    }

    private void FixedUpdate()
    {
        Move(horizontalMove, verticalMove);
    }

    private void Move(float x, float y)
    {
        rb.velocity = new Vector2(x, y);

        if (x > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (x < 0 && m_FacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            curHealth -= damage;
        }       
    }

    private void Die()
    {
        if (!isDead)
        {
            anim.SetBool("IsDead", true);
            isDead = true;
            horizontalMove = verticalMove = 0;
            rb.mass = 1;
            rb.gravityScale = 30;
            playDeathSound = true;
            if (playDeathSound)
            {
                AudioSource.PlayClipAtPoint(PlayDeathSound(), gameObject.transform.position);
                playDeathSound = false;
            }
        }          
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            curHealth = 0;
            rb.AddForce(new Vector2(deathThrust, deathThrust), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(deathThrust, deathThrust), ForceMode2D.Impulse);
            PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if (!player.invulnerable)
            {
                player.TakeDamage(damage);
                player.AddHitImpact();
            }   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            curHealth = 0;
            rb.AddForce(new Vector2(deathThrust, deathThrust), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(deathThrust, deathThrust), ForceMode2D.Impulse);
        }
    }

    private AudioClip PlayDeathSound()
    {
        var sound = Random.Range(0, deathSounds.Length);
        var soundToPlay = deathSounds[sound];
        return soundToPlay;
    }

    //Called from RespawnButton script
    public void Respawn()
    {
        anim.SetBool("IsDead", false);
        isDead = false;
        transform.position = startPos;
        gameObject.SetActive(true);
        curHealth = maxHealth;
        horizontalMove = res_hMove;
        verticalMove = res_vMove;
        rb.mass = res_mass;
        rb.gravityScale = res_grav;
    }

}
