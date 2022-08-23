using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float playerMaxHealth = 3f;
    public float playerCurHealth;
    Animator anim;
    Rigidbody2D rb;
    CapsuleCollider2D col;
    
    public float thrust = 1f;
    public bool isDead = false;
    public bool invulnerable = false;

    public GameObject killVolume;
    public GameObject spawnPoint;

    FadeUI deathCanvas;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        playerCurHealth = playerMaxHealth;
        deathCanvas = GameObject.Find("DeathCanvas").GetComponent<FadeUI>();
    }

    void Update()
    {
        if (playerCurHealth <= 0)
        {
            Die();
        }

        if (transform.position.y <= killVolume.transform.position.y)
        {
            playerCurHealth = 0;
            Die();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "BossWeapon")
        {
            BossFight bf = collision.collider.GetComponentInParent<BossFight>();
            if (!invulnerable && bf.canDamagePlayer == true && !isDead)
            {
                TakeDamage(bf.damage);
            }
        }

        if (collision.collider.tag == "EnvironmentalHazard")
        {
            if (!isDead)
            {
                gameObject.GetComponent<PlayerMovement>().enabled = false;
                rb.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
                playerCurHealth = 0;
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpawnPoint")
        {
            spawnPoint = collision.gameObject;
        }

        if (collision.tag == "HealthPickup")
        {
            if (playerCurHealth < playerMaxHealth)
            {
                if (playerCurHealth + 1 > playerMaxHealth)
                {
                    playerCurHealth += (playerMaxHealth - playerCurHealth);
                }
                else
                {
                    playerCurHealth += 1;
                }

            }
        }
    }

    //Called from EnemyController script
    public void TakeDamage(float damage)
    {
        if (playerCurHealth > 0)
        {
            invulnerable = true;
            anim.SetBool("TakeDamage", true);
            playerCurHealth -= damage;
        }
    }

    //Called from EnemyController script
    public void AddHitImpact()
    {
        Debug.Log("test");
        rb.AddForce(-transform.right * thrust, ForceMode2D.Impulse);
        rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
    }

    //Called from anim event in Hit anim
    private void StopDamage()
    {
        anim.SetBool("TakeDamage", false);
        invulnerable = false;
    }

    private void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", true);
        anim.SetBool("TakeDamage", false);
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsAttacking", false);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(deathCanvas.Fade());

    }

    //Called from RespawnButton script
    public void Respawn()
    {
        transform.position = spawnPoint.transform.position;
        isDead = false;
        anim.SetBool("IsDead", false);
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        playerCurHealth = playerMaxHealth;
        invulnerable = false;
    }
    
}
