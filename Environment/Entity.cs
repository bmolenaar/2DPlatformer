using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public AnimationCurve myCurve;
    public AudioClip pickupSound;

    Keyframe[] keys;
    Vector2 startPos;

    private void Start()
    {
        keys = myCurve.keys;
        keys[0].value = transform.position.y - 0.5f;
        keys[1].value = transform.position.y;
        myCurve.keys = keys;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        gameObject.SetActive(false);
    }
}
