using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingEnemy : Enemy
{
    [SerializeField] Transform Right;
    [SerializeField] Transform Left;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    Rigidbody2D rb;
    int direction;

    private void Move(int direction)
    {
        rb.AddForce(new Vector2(direction, 0f) * acceleration);
        if (Mathf.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)
                * (speed), rb.velocity.y);
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
    }
    private void Update()
    {
        if (Health <= 0)
        {
            base.Death();
        }

        if (transform.position.x >= Right.position.x)
        {
            direction = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (transform.position.x <= Left.position.x)
        {
            direction = 1;
            transform.localScale = new Vector3(1, 1, 1);


        }
    }
    private void FixedUpdate()
    {
        Move(direction);
    }
}
