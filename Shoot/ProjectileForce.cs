using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForce : MonoBehaviour
{
    public float force = 10f;
    Rigidbody2D rb;
    private float lifetime;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        rb.velocity = -transform.up * force;
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime > 1.5f)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D hitObject = collision.GetComponent<Collider2D>();
        if (hitObject.tag != "Player")
        {
            GameObject.Destroy(gameObject);
        }
    }
}
