using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Minotaur : BossFight
{

    bool shouldMove = false;
    bool m_FacingRight = false;

    public AudioClip stompSound;
    public AudioClip roarSound;
    
    public GameObject player;

    private void Update()
    {
        if (curHealth <= 0)
        {
            Die();
        }

        if (player.transform.position.x >= trigger.transform.position.x && !fightTriggered)
        {
            fightTriggered = true;
            StartCoroutine(SetTrigger());
        }
        else if (player.transform.position.x <= trigger.transform.position.x)
        {
            trigger.SetActive(false);
            if (fightTriggered)
            {
                fightTriggered = false;
            }
        }


        if (player.transform.position.x >= transform.position.x)
        {
            m_FacingRight = true;
        }
        else
        {
            m_FacingRight = false;
        }

        if (fightTriggered)
        {
            anim.SetBool("FightTriggered", true);
            if (shouldMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

                if (m_FacingRight)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else if (!m_FacingRight)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }

            if (Vector2.Distance(transform.position, player.transform.position) < 2)
            {
                anim.SetBool("Attack", true);
            }
        }
    }

    //Called from Animation Event within Roar anim (Minotaur)
    void PlayStompSound()
    {
        AudioSource.PlayClipAtPoint(stompSound, transform.position);
    }

    //Called from Animation Event within Roar anim (Minotaur)
    void PlayRoarSound()
    {
        AudioSource.PlayClipAtPoint(roarSound, transform.position);
    }

    void Die()
    {
        anim.SetBool("IsDead", true);
        shouldMove = false;
    }

    //Called from Animation Event within Roar anim (Minotaur)
    void MoveToPlayer()
    {
        anim.SetBool("StartMove", true);
        shouldMove = true;
    }

    IEnumerator SetTrigger()
    {
        yield return new WaitForSecondsRealtime(1f);
        trigger.SetActive(true);
    }
}
