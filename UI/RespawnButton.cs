using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    PlayerHealth playerHealth;
    Canvas deathCanvas;

    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> pickups = new List<GameObject>();

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        deathCanvas = GameObject.Find("DeathCanvas").GetComponent<Canvas>();

        foreach (var g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(g);
        }

        foreach (var p in GameObject.FindGameObjectsWithTag("HealthPickup"))
        {
            pickups.Add(p);
        }
    }

    public void ResetLevel()
    {
        PlayerRespawn();
        ResetUI();
        ResetEnemies();
        ResetBoss();
        //ResetPickups();
    }

    void PlayerRespawn()
    {
        playerHealth.Respawn();
        
    }

    void ResetUI()
    {
        deathCanvas.GetComponent<CanvasGroup>().alpha = 0;
        deathCanvas.GetComponent<FadeUI>().respawnButton.interactable = false;
    }

    void ResetEnemies()
    {
        foreach (var enemy in enemies)
        {
            EnemyController ctrl = enemy.GetComponent<EnemyController>();
            ctrl.Respawn();
        }
    }

    private void ResetBoss()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        BossFight bf = boss.GetComponent<BossFight>();
        bf.Respawn();
    }

    private void ResetPickups()
    {
        foreach (var item in pickups)
        {
            item.SetActive(true);
        }
    }


}
