using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    Vector2 startPos;

    public Animator anim;   
    public GameObject trigger;

    public float moveSpeed = 5f;
    public float maxHealth = 5f;
    public float curHealth;
    public float damage = 1f;

    public bool fightTriggered = false;
    public bool canDamagePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        startPos = transform.position;
        anim = this.GetComponent<Animator>();
    }


    public void TakeDamage(float damage)
    {
        if (curHealth > 0)
        {
            curHealth -= damage;
        }
        
    }

    //Called from RespawnButton script
    public void Respawn()
    {
        transform.position = startPos;
        curHealth = maxHealth;
        anim.SetBool("FightTriggered", false);
        anim.SetBool("StartMove", false);
        anim.SetBool("Attack", false);
        trigger.SetActive(false);
        fightTriggered = false;
    }

    //Called from Animation Event within Attack anim (Minotaur)
    void DamagePlayer()
    {
        canDamagePlayer = true;
    }

    //Called from Animation Event within Attack anim (Minotaur)
    void StopAttack()
    {
        anim.SetBool("Attack", false);
        canDamagePlayer = false;
    }

}
