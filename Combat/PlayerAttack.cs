using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    float slashCounter;
    float enemyTakeDmgCounter;
    public float cooldown = 0.3f;
    public float slashDamage = 1f;

    public AudioClip[] slashSounds;

    PlayerHealth playerHealth;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        playerHealth = this.GetComponent<PlayerHealth>();
    }


    void Update()
    {
        slashCounter += Time.deltaTime;
        enemyTakeDmgCounter += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && slashCounter >= cooldown && !playerHealth.isDead)
        {
            anim.SetBool("IsAttacking", true);
            slashCounter = 0;
            AudioSource.PlayClipAtPoint(PlaySlashSound(), gameObject.transform.position);
        }
    }

    //Called from Animation Event in Attack anim
    void StopAttack()
    {
        anim.SetBool("IsAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D hitObject = collision.GetComponent<Collider2D>();

        if (hitObject.tag == "Enemy")
        {
            hitObject.GetComponent<EnemyController>().TakeDamage(slashDamage);
        }

        if (hitObject.tag == "Boss")
        {
            if (enemyTakeDmgCounter >= 0.5f)
            {
                hitObject.GetComponent<BossFight>().TakeDamage(slashDamage);
                enemyTakeDmgCounter = 0;
            }
        }

    }

    private AudioClip PlaySlashSound()
    {
        var sound = Random.Range(0, slashSounds.Length);
        var soundToPlay = slashSounds[sound];
        return soundToPlay;
    }

}
