using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    Animator anim;
    public GameObject projectile;
    public Transform spawnLoc;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("IsShooting", true);
        }
    }

    void StopShoot()
    {
        anim.SetBool("IsShooting", false);
    }

    void SpawnProjectile()
    {
        GameObject.Instantiate(projectile, spawnLoc.position,spawnLoc.rotation);
    }
}
