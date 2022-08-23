using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject destroyable;
    public Sprite flipped;
    public AudioClip flipSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(flipSound, transform.position);
            gameObject.GetComponent<SpriteRenderer>().sprite = flipped;
            GameObject.Destroy(destroyable);
        }
    }
}
