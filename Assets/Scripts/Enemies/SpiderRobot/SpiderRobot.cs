using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRobot : Enemy
{
    Rigidbody2D rb;



    [SerializeField] float agroDistanceX;
    [SerializeField] float agroDistanceY;
    [SerializeField] float targetMinHeight;
    [SerializeField] float AttackBuildUpTime;
    [SerializeField] int MeleeDamage;
    [SerializeField] float shakeAmount;


    [SerializeField] float attackForceY;
    [SerializeField] float attackForceX;


    bool _facingRight;
    bool AttackRequested;
    float AttackBuildUpTimer;

    [Header("Connection to other Script")]
    private Player player;
    Transform playerTransform;
    Vector2 startpos;





    [SerializeField] Animator animator;
    bool isAttacking;
    SpriteRenderer Sprite;

    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] Transform[] groundCheck;
    bool canAttackAgain;
    bool isTakingDamage;

    enum AnimationCases
    {
        Idle,
        TakingDamage,
        BuildUp,
        Attack,
        Landing,
    }
    AnimationCases animationCases;







    private void Start()
    {
        animationCases = AnimationCases.Idle;

        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        rb = GetComponent<Rigidbody2D>();
        canAttackAgain = true;
        Sprite = GetComponent<SpriteRenderer>();
    }



    private void Update()
    {
        if (isTakingDamage)
        {
            AttackBuildUpTimer = 0;
            transform.position = startpos + Random.insideUnitCircle * shakeAmount;
        }
        HandlingAnimation();
        if (Health <= 0)
        {
            Death();
        }


    }



    private void FixedUpdate()
    {
        if (AttackRequested)
        {
            isAttacking = true;
            JumpAttack();
            AttackRequested = false;
        }
    }


    void Request_Attack_When_Build_Up_Time_had_Passed()
    {
        if (AttackBuildUpTimer < AttackBuildUpTime)
        {
            if (canAttackAgain)
            {
                AttackBuildUpTimer += Time.deltaTime;
            }
            else
            {
                AttackBuildUpTimer = 0;
            }

        }
        else
        {
            AttackBuildUpTimer = 0;
            AttackRequested = true;
            canAttackAgain = false;
        }
    }


    void HandlingAnimation()
    {
        if (isTakingDamage)
        {
            animationCases = AnimationCases.TakingDamage;
        }




        switch (animationCases)
        {
            case AnimationCases.Idle:
                animator.Play("Idle");
                if (CanSeePlayer())
                {
                    AttackBuildUpTimer = 0;
                    animationCases = AnimationCases.BuildUp;
                }
                break;

            case AnimationCases.BuildUp:
                if (!CanSeePlayer())
                {
                    animationCases = AnimationCases.Idle;

                }
                else 
                {
                    if (AttackBuildUpTimer < AttackBuildUpTime * 0.1f)
                    {
                        LookAtTarget();
                    }
                    Request_Attack_When_Build_Up_Time_had_Passed();
                    //once Attack is Requested is Attacking will become True
                    if (isAttacking)
                    {
                        animationCases = AnimationCases.Attack;
                        
                    }
                    else
                    {
                        animator.Play("AttackBuildUp");
                    }
                }
                break;
            case AnimationCases.Attack:
                if (isAttacking)
                {
                    animator.Play("Attack");
                    StartCoroutine(IsAttackingToFalse());
                }
                else
                {
                    animationCases = AnimationCases.BuildUp;
                }
                

                break;
            case AnimationCases.Landing:
                break;
            case AnimationCases.TakingDamage:
                if (isTakingDamage)
                {
                    animator.Play("TakingDamage");
                    canAttackAgain = true;
                    AttackBuildUpTimer = 0;
                    isAttacking = false;
                }
                else
                {
                    animationCases = AnimationCases.Idle;

                }
                break;
            default:
                break;
        }


    }


    public void CanAttackAgainToTrue()
    {
        canAttackAgain = true;
    }


    IEnumerator IsAttackingToFalse()
    {
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
        canAttackAgain = true;
    }






// TOOLS 


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
        //yield return new WaitForSeconds(inBetweenTime);
        //Sprite.color = Color.red;
        //yield return new WaitForSeconds(inBetweenTime);
        //Sprite.color = Color.white;
        isTakingDamage = false;
        transform.position = startpos;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            if (player != null)
            {
                player.TakeDamage(MeleeDamage);
            }
        }
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
    private void JumpAttack()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * attackForceY);

        if (_facingRight)
        {
            rb.AddForce(Vector2.right * attackForceX);
        }
        else
        {
            rb.AddForce(Vector2.left * attackForceX);
        }
    }
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
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        isTakingDamage = true;
        startpos = transform.position;
        StartCoroutine(IsTakingDamagetoFalse(0.04f));

    }

}
