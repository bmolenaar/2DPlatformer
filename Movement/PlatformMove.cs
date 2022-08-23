using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float horizontalMove;
    public float verticalMove;
    public float moveDistance;

    Vector2 startPos;

    bool playerContact;

    void Start()
    {
        startPos = this.transform.position;
        horizontalMove *= moveSpeed;
        verticalMove *= moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x < startPos.x - moveDistance)
        {
            horizontalMove = 1 * moveSpeed;
        }
        else if (this.transform.position.x > startPos.x + moveDistance)
        {
            horizontalMove = -1 * moveSpeed;
        }

        if (this.transform.position.y < startPos.y - moveDistance)
        {
            verticalMove = 1 * moveSpeed;
        }
        else if (this.transform.position.y > startPos.y + moveDistance)
        {
            verticalMove = -1 * moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
    }

    private void Move(float x, float y)
    {
        transform.position += new Vector3(x, y, 0);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerContact = true;
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerContact = false;
            collision.collider.transform.SetParent(null);
        }
    }
}
