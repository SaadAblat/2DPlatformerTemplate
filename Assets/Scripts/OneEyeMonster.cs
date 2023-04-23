using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEyeMonster : Enemy
{
    [SerializeField] float AgroDistance_X;
    [SerializeField] float Acceleration;
    [SerializeField] float Speed;
    [SerializeField] Animator animator;
    private Rigidbody2D rb;
    bool lookingRight;
    //[SerializeField] GameObject BloodExplosionPrefab;

    [SerializeField] Transform groundCheckUp;

    [SerializeField] Transform[] groundCheck;
    [SerializeField] LayerMask whatisGround;
    [SerializeField] Player player;
    [SerializeField] int MeleeDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lookingRight = false;

    }

    protected override void Death()
    {
        Instantiate(BloodExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

                if (player != null)
                {
                    if (!IsGrounded())
                    {
                        FacePlayer();
                    }
                    if (Health <= 0)
                    {
                        Death();
                    }
                }

    }
    private void FixedUpdate()
    {
        Flip();
        if (player != null)
        {

            if (playerInAgroDistance())
            {
                if (IsGrounded())
                {
                    Move(Direction());
                }
                else
                {
                    Idle();
                }
            }
            else
            {
                Idle();
            }
        }
        else
        {
            Idle();
        }

    }

    void Move(Vector2 direction)
    {
        animator.SetFloat("Speed", 1);
        if (IsGrounded())
        {
            rb.AddForce(Acceleration * direction);
            if (Mathf.Abs(rb.velocity.x) > Speed)
            {
                rb.velocity = direction * Speed;
            }
        }
    }
    void Idle()
    {
        animator.SetFloat("Speed", 0);
    }


    void Flip()
    {
        if (rb.velocity.x > 0 && !lookingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            lookingRight = true;

        }
        else if (rb.velocity.x < 0 && lookingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            lookingRight = false;

        }
    }
    void FacePlayer()
    {
        if (lookingRight && player.transform.position.x < transform.position.x)
        {
            transform.Rotate(0f, 180f, 0f);
            lookingRight = false;
        }
        else if (!lookingRight && player.transform.position.x > transform.position.x)
        {
            transform.Rotate(0f, 180f, 0f);
            lookingRight = true;
        }
    }


    bool IsGrounded()
    {


        foreach (Transform groundcheck in groundCheck)
        {
            if (Physics2D.Linecast(groundCheckUp.position, groundcheck.position, whatisGround))
            {
                return true;
            }
        }
        return false;
    }
    bool playerInAgroDistance()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= AgroDistance_X)
        {
            return true;
        }
        return false;
    }

    Vector2 Direction()
    {
        if (player.transform.position.x < transform.position.x)
        {
            return Vector2.left;
        }
        return Vector2.right;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(MeleeDamage);
            }
        }
    }

}
