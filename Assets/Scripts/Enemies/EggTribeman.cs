using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTribeman : Enemy
{
    Rigidbody2D rb;



    [SerializeField] float agroDistanceX;
    [SerializeField] float agroDistanceY;
    [SerializeField] float MeleeDistancex;
    [SerializeField] float MeleeDistancey;

    [SerializeField] float targetMinHeight;
    [SerializeField] float AttackBuildUpTime;
    [SerializeField] int MeleeDamage;
    [SerializeField] float shakeAmount;


    //[SerializeField] float attackForceY;
    //[SerializeField] float attackForceX;


    bool _facingRight;
    //bool AttackRequested;
    //float AttackBuildUpTimer;


    [SerializeField] float Speed;
    [SerializeField] float MaxSpeed;
    [SerializeField] float RandomConstant;

    [Header("Connection to other Script")]
    private Player player;
    Transform playerTransform;
    Vector2 startpos;


    [SerializeField] Animator animator;
    bool isAttacking;
    [SerializeField] SpriteRenderer Sprite;

    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] private LayerMask whatisObstacle;
    [SerializeField] Transform[] groundCheck;

    [SerializeField] Transform obstacleCheckStart;
    [SerializeField] Transform[] obstacleCheck;
    bool canAttackAgain;
    bool isTakingDamage;
    
    bool move;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health<=0)
        {
            Death();
        }
        if (isTakingDamage)
        {
            move = false;
            animator.Play("TakingDamage");
        }
        else
        {
            if (!IsReturningBackToStartPos)
            {
                if (IsInPosition())
                {
                    HandleWhenIsInPosition();
                }
                else
                {
                    IsReturningBackToStartPos = true;
                }
            }
            else
            {
                ReturnBackToStartPosition();
            }


            

        }

        ManageDrag();

    }

    void HandleWhenIsInPosition()
    {
        if (CanSeePlayer())
        {
            if (PlayerInMelee())
            {
                animator.Play("Attack");
                isAttacking = true;
                move = false;

            }
            else
            {
                if (!isAttacking)
                {
                    if (IsGrounded() && !IsObstacleInFront())
                    {
                        animator.Play("Run");
                        move = true;
                    }
                    else
                    {
                        move = false;
                        animator.Play("Idle");
                    }
                    LookAtTarget();

                }

            }
        }
        else
        {
            move = false;
            animator.Play("Idle");
            isAttacking = false;
        }
    }

    [SerializeField] Transform left;
    [SerializeField] Transform right;
    Vector2 startPos;
    bool backToStartPos;
    bool IsReturningBackToStartPos;
    void ReturnBackToStartPosition()
    {
        //is way too left
        if (transform.position.x < startPos.x)
        {
            if (!_facingRight)
            {
                ChangeDirection();
            }
            if (IsGrounded() && !IsObstacleInFront())
            {
                animator.Play("Run");
                move = true;
            }
            else
            {
                move = false;
                animator.Play("Idle");
            }
            if (transform.position.x >= startPos.x - 0.2f)
            {
                IsReturningBackToStartPos = false;
            }
        }
        // is way too right
        else if (transform.position.x > startPos.x)
        {
            if (_facingRight)
            {
                ChangeDirection();
            }
            if (IsGrounded() && !IsObstacleInFront())
            {
                animator.Play("Run");
                move = true;
            }
            else
            {
                move = false;
                animator.Play("Idle");
            }
            if (transform.position.x <= startPos.x + 0.2f)
            {
                IsReturningBackToStartPos = false;
            }
        }
    }
    bool IsInPosition()
    {
        if (transform.position.x <= right.position.x && transform.position.x >= left.position.x)
        {
            return true;
        }
        return false;
    }

    public void IsAttackingTofalse()
    {
        if (!PlayerInMelee())
        {
            isAttacking = false;
        }
    }
    private void FixedUpdate()
    {
        if (move)
        {
            Move();
        }
    }


    void Move()
    {
        float random = Random.Range(-RandomConstant, RandomConstant);
        float random2 = Random.Range(-RandomConstant*20, RandomConstant*20);
        if (_facingRight)
        {
            rb.AddForce(Vector2.right * (Speed + random2));
            if (rb.velocity.x >= (MaxSpeed + random))
            {
                rb.velocity = Vector2.right * (MaxSpeed + random);
            }
        }
        else
        {
            rb.AddForce(Vector2.left * (Speed + random2));
            if (rb.velocity.x <= (MaxSpeed + random))
            {
                rb.velocity = Vector2.left * (MaxSpeed + random);
            }

        }

    }

    void ManageDrag()
    {
        if (IsGrounded())
        {
            rb.drag = 10;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAttacking)
        {
            if (player != null)
            {
                player.TakeDamage(MeleeDamage);
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Death();
        }
    }

    private void ChangeDirection()
    {
        _facingRight = !_facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * 1, transform.localScale.z * 1);
    }
    private void LookAtTarget()
    {
        if (player != null)
        {
            float DirX = playerTransform.position.x - transform.position.x;

            if (DirX > 0 && !_facingRight || DirX < 0 && _facingRight)
            {
                ChangeDirection();
            }
        }
    }
    private bool CanSeePlayer()
    {
        if (playerTransform != null)
        {
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) < agroDistanceX
        && Mathf.Abs(playerTransform.position.y - transform.position.y) < agroDistanceY
        && (playerTransform.position.y - transform.position.y) > targetMinHeight)

            {
                return true;
            } 
        }
        return false;
    }

    private bool PlayerInMelee()
    {
        if (playerTransform != null)
        {
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) < MeleeDistancex
        && Mathf.Abs(playerTransform.position.y - transform.position.y) < MeleeDistancey
        && (playerTransform.position.y - transform.position.y) > targetMinHeight)

            {
                return true;
            } 
        }
        return false;
    }
    //private void JumpAttack()
    //{
    //    rb.velocity = Vector2.zero;
    //    rb.AddForce(Vector2.up * attackForceY);

    //    if (_facingRight)
    //    {
    //        rb.AddForce(Vector2.right * attackForceX);
    //    }
    //    else
    //    {
    //        rb.AddForce(Vector2.left * attackForceX);
    //    }
    //}
    bool IsGrounded()
    {
        foreach (Transform groundcheck in groundCheck)
        {
            if (Physics2D.Linecast(transform.position, groundcheck.position, whatisGround))
            {
                return true;

            }
        }
        return false;
    }
    bool IsObstacleInFront()
    {
        foreach (Transform obstacl in obstacleCheck)
        {
            if (Physics2D.Linecast(obstacleCheckStart.position, obstacl.position, whatisObstacle))
            {
                return true;

            }
        }
        return false;
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        isTakingDamage = true;
        isAttacking = false;
        startpos = transform.position;
        StartCoroutine(IsTakingDamagetoFalse(0.06f));

    }
    IEnumerator IsTakingDamagetoFalse(float inBetweenTime)
    {
        Sprite.color = Color.red;
        yield return new WaitForSeconds(inBetweenTime);
        Sprite.color = Color.white;
        yield return new WaitForSeconds(inBetweenTime);
        Sprite.color = Color.red;
        yield return new WaitForSeconds(inBetweenTime);
        Sprite.color = Color.white;
        yield return new WaitForSeconds(inBetweenTime);
        Sprite.color = Color.red;
        yield return new WaitForSeconds(inBetweenTime);
        Sprite.color = Color.white;
        isTakingDamage = false;
        //transform.position = startpos;


    }

}
