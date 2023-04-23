using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private IWeaponInterface weaponInterface;

    bool PickAxeAttackRequested;
    float meleeTimer;
    [SerializeField] float TimeBeforeCanPerformMeleeAgain;

    Rigidbody2D rb;


    [SerializeField] Animator player_Animator;
    [SerializeField] SpriteRenderer playerSprite;

    /// <summary>
    /// The transform of the other arm while holding the pistol so I can flip it in the flip function
    /// </summary>
    [SerializeField] Transform playerOtherArm;

    bool isAttacking;
    bool isTakingDamage;

    internal float horizontal;
    internal float Vertical;
    bool jumpRequested = false;

    [SerializeField]
    internal bool lookingRight;

    [Header("Movement Variables")]
    [SerializeField]
    float Acceleration;

    [SerializeField]
    float RunSpeed;

    [SerializeField]
    float groundLinearDrag;
    [SerializeField]
    float airlinearDrag;
    bool changingDir => (rb.velocity.x > 0f && horizontal < 0f ||
    rb.velocity.x < 0f && horizontal > 0f);


    [Header("Jump Variables")]
    [SerializeField]
    float PlayerVerticalJumpForce;
    [SerializeField]
    float PlayerHorizontalJumpForce;
    [SerializeField]
    float hangTime;
    float hangTimeCounter;
    [SerializeField]
    float jumpTime;
    float jumpTimeCounter;
    [SerializeField]
    float jumpPressedRememberTime;
    float jumpPressedRememberTimer;
    bool isJumping;
    [SerializeField]
    float _minSpeedtoFall;
    [SerializeField]
    float Max_Vertical_AirSpeed;
    [SerializeField]
    float AirAcceleration;
    [SerializeField] float Max_Horizontal_AirSpeed;

    bool keepPushingJump;

    [SerializeField]
    float fallMultipier;
    [SerializeField]
    float lowJumpMultipier;


    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] Transform[] groundCheck;
    bool hasLanded;

    [Header("Camera Variables")]
    [SerializeField] float ShakeIntensity;
    [SerializeField] float ShakeTime;
    [SerializeField] float ShakeFrequency;



    [Header("Weapon Variables")]
    [SerializeField] Weapon_Pistol PistolPrefab;
    [SerializeField] Weapon_Shootgun ShotgunPrefab;
    [SerializeField] internal bool havePistol;
    [SerializeField] internal bool haveShotgun;



    internal Vector3 LookDirection;
    Camera maincamera;
    [SerializeField] Transform playerGraphic;

    public enum WeaponType
    {
        unarmed,
        pistol,
        Shotgun,
        pickAxe
    }
    WeaponType weaponType;
    WeaponType LastweaponEquiped;


    internal int Health;
    [SerializeField] int maxHealth;
    [SerializeField] HealthSlider healthSlider;
    [SerializeField] TMP_Text AmmoCountText;

    [SerializeField] internal int ShotgunAmmoCount;
    [SerializeField] internal int MaxShotgunAmmo;

    [SerializeField] internal int PistolAmmoCount;
    [SerializeField] internal int MaxPistolAmmo;


    [SerializeField] GameObject LandDust;
    [SerializeField] Transform dustPos;
    [SerializeField] GameObject JumpTrail;

    internal bool isOnLadder;
    internal bool isClimbing;

    [SerializeField] Transform startPos;

    [SerializeField] string takeDamageSound;
    // Start is called before the first frame update
    void Start()
    {
        if (!SaveData.SaveInstance.firstCheckPointActivated)
        {
            transform.position = startPos.position;
        }
        else
        {
            transform.position = SaveData.SaveInstance.LastCheckPointPosition;
            PistolAmmoCount = SaveData.SaveInstance.LastCheckPointPistolAmmo;
            ShotgunAmmoCount = SaveData.SaveInstance.LastCheckPointShotgunAmmo;
            havePistol = SaveData.SaveInstance.HavePistol;
            haveShotgun = SaveData.SaveInstance.HaveShotgun;
        }
        
        weaponInterface = PistolPrefab;
        weaponType = WeaponType.unarmed;
        Health = maxHealth;
        healthSlider.SetMaxHealth(maxHealth);
        healthSlider.SetHealth(Health);
        rb = GetComponent<Rigidbody2D>();
        maincamera = Camera.main;
        Physics2D.IgnoreLayerCollision(0, 1);
    }


    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Death();
        }

        healthSlider.SetHealth(Health);
        

        if (!isTakingDamage)
        {
            GetInput();
            if (!isAttacking)
            {
                Flip();

            }
        }
        HandleControllersCases();
        HandleWeapons();
        HandleAnimation();


        if (IsGrounded())
        {
            if (!hasLanded)
            {
                JumpTrail.SetActive(false);

                trompolineJump = false;
                hasLanded = true;
                
                //AudioManager.instance.Play("Land");
                CinemachineShake.CameraInstance.ShakeCamera(ShakeIntensity, ShakeTime, ShakeFrequency);
                Instantiate(LandDust, dustPos.position, Quaternion.identity);
            }
        }
        else
        {
            JumpTrail.SetActive(true);
            hasLanded = false;
        }
        if (!isClimbing)
        {
            FallMultiplier();
        }

    }
    private void HandleControllersCases()
    {
        if (isOnLadder && Vertical != 0)
        {
            isClimbing = true;
        }
        else if (!isOnLadder)
        {
            isClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            HandleLadder();
        }
        else
        {
            HandleMovement();
        }
    }


    private void HandleLadder()
    {
        rb.gravityScale = 0;
        rb.drag = 6f;
        rb.AddForce(new Vector2(horizontal, Vertical) * Acceleration);
        if (Mathf.Abs(rb.velocity.x) > RunSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * RunSpeed, rb.velocity.y);
        }
        if (Mathf.Abs(rb.velocity.y) > RunSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * RunSpeed);
        }
    }
    private void FallMultiplier()
    {
        if (rb.velocity.y < _minSpeedtoFall)
        {
            rb.velocity += (fallMultipier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !trompolineJump)
        {
            rb.velocity += (lowJumpMultipier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
    }
    private void GroundLinearDrag()
    {
        // this makes a difference in jump high between while moving vs jumping without moving, cause the groundlinear drag get activated when still 
        if (Mathf.Abs(horizontal) < 0.4f || changingDir)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    bool trompolineJump;
    internal void Trompoline()
    {
        //jumpRequested = true;
        
        Jump(PlayerVerticalJumpForce*3, 0 );
        trompolineJump = true;
    }
    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        jumpPressedRememberTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressedRememberTimer = jumpPressedRememberTime;
        }
        if (jumpPressedRememberTimer > 0 && hangTimeCounter > 0)
        {
            jumpRequested = true;
        }
        else
        {
            isJumping = false;

        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                keepPushingJump = true;
            }
            else
            {
                keepPushingJump = false;
                isJumping = false;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            keepPushingJump = false;

        }
        if (Input.GetMouseButtonDown(0))
        {
            weaponInterface.ShootRequestTotrue();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (meleeTimer > TimeBeforeCanPerformMeleeAgain)
            {
                PickAxeAttackRequested = true;
                meleeTimer = 0;
            }
        }

    }
    void AxeAttack()
    {
        meleeTimer += Time.deltaTime;
        if (PickAxeAttackRequested )
        {


            weaponType = WeaponType.pickAxe;
            PickAxeAttackRequested = false;
            isAttacking = true;
            player_Animator.SetBool("PickAxeAttack", true);
            StopAllCoroutines();
            StartCoroutine(BackToLastWeapon(0.55f));
        }
        else
        {
            player_Animator.SetBool("PickAxeAttack", false);
        }
    }

    IEnumerator BackToLastWeapon(float time)
    {
        yield return new WaitForSeconds(time);
        weaponType = LastweaponEquiped;
        isAttacking = false;
        
        //yield return new WaitUntil(() => IsGrounded());
        
        //weaponType = LastweaponEquiped;

    }
    bool isHittingGround;
    IEnumerator isHittingGroundToFalse(float time)
    {
        yield return new WaitForSeconds(time);
        isHittingGround = false;

    }
    
    IEnumerator ChangeWeaponUntilTheAttackIsDone_Pistol()
    {
        yield return new WaitUntil(() => !isAttacking);
        weaponType = WeaponType.pistol;
        LastweaponEquiped = WeaponType.pistol;
    }
    IEnumerator ChangeWeaponUntilTheAttackIsDone_Shotgun()
    {
        yield return new WaitUntil(() => !isAttacking);
        weaponType = WeaponType.Shotgun;
        LastweaponEquiped = WeaponType.Shotgun;
    }

    void HandleWeapons()
    {
        AxeAttack();

        if (Input.GetKeyDown(KeyCode.Alpha1) && havePistol)
        {
            if (!isAttacking)
            {
                weaponType = WeaponType.pistol;
                LastweaponEquiped = WeaponType.pistol;
            }
            else
            {
                StartCoroutine(ChangeWeaponUntilTheAttackIsDone_Pistol());
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && haveShotgun)
        {
            if (!isAttacking)
            {
                weaponType = WeaponType.Shotgun;
                LastweaponEquiped = WeaponType.Shotgun;
            }
            else
            {
                StartCoroutine(ChangeWeaponUntilTheAttackIsDone_Shotgun());
            }
        }

        switch (weaponType)
        {
            case WeaponType.unarmed:
                PistolPrefab.transform.parent.gameObject.SetActive(false);
                ShotgunPrefab.gameObject.SetActive(false);
                break;
            case WeaponType.pistol:
                PistolPrefab.transform.parent.gameObject.SetActive(true);
                weaponInterface = PistolPrefab;
                AmmoCountText.text = "X " + PistolAmmoCount +"/ " + MaxPistolAmmo;

                ShotgunPrefab.gameObject.SetActive(false);

                break;
            case WeaponType.Shotgun:
                ShotgunPrefab.gameObject.SetActive(true);
                weaponInterface = ShotgunPrefab;
                AmmoCountText.text = "X " + ShotgunAmmoCount + "/ " + MaxShotgunAmmo;

                PistolPrefab.transform.parent.gameObject.SetActive(false);
                break;
            case WeaponType.pickAxe:


                PistolPrefab.transform.parent.gameObject.SetActive(false);
                ShotgunPrefab.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    void HandleAnimation()
    {
        if (!isTakingDamage)
        {
            if (!isAttacking)
            {
                if (IsGrounded())
                {
                    if (!hasLanded)
                    {
                        isHittingGround = true;
                        StartCoroutine(isHittingGroundToFalse(0.15f));
                    }
                    if (isHittingGround)
                    {
                        player_Animator.Play("HitGround");
                    }
                    else
                    {
                        if (Mathf.Abs(horizontal) > 0.1f)
                        {
                            player_Animator.Play("Run");
                        }
                        else
                        {
                            player_Animator.Play("Idle");
                        }
                    }
                }
                else
                {
                    if (rb.velocity.y > 0)
                    {
                        player_Animator.Play("Jump");
                    }
                    else
                    {

                        player_Animator.Play("Land");
                    }

                }
            }
            else
            {
                player_Animator.Play("PickAxeAttack");
                isHittingGround = false;


            }
        }
        else
        {
            player_Animator.Play("TakingDamage");

        }




        //if (PickAxeAttackRequested)
        //{
        //    PickAxeAttackRequested = false;
        //    player_Animator.SetBool("PickAxeAttack", true) ;
        //}
        //else
        //{
        //    player_Animator.SetBool("PickAxeAttack", false);
        //}
    }


    private void OnEnable()
    {
        isHittingGround = false;
        isAttacking = false;
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Follow = transform;
    }
    void HandleMovement()
    {

        Clamp_Vertical_Velocity();
        rb.gravityScale = 1;
        if (IsGrounded())
        {
            if (!isAttacking)
            {
                Run(RunSpeed);
            }
            else
            {
                Run(RunSpeed / 2);
            }
            hangTimeCounter = hangTime;
            if (!jumpRequested)
            {
                GroundLinearDrag();
            }
            else
            {
                AirLinearDrag();

            }
        }
        else
        {
            HandleJumpAttack();
            AirRun();
            AirLinearDrag();
        }
        hangTimeCounter -= Time.fixedDeltaTime;
        if (jumpRequested && !isAttacking)
        {
            Jump(PlayerVerticalJumpForce, PlayerHorizontalJumpForce);
        }
        else
        {
            jumpRequested = false;
        }

        if (keepPushingJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * PlayerVerticalJumpForce, ForceMode2D.Impulse);

            jumpTimeCounter -= Time.fixedDeltaTime;
        }
    }

    void Clamp_Vertical_Velocity()
    {
        if (Mathf.Abs(rb.velocity.y) > Max_Vertical_AirSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * Max_Vertical_AirSpeed);
        }
    }
    void HandleJumpAttack()
    {
        if (isAttacking)
        {
            StartCoroutine(JumpAttack());
        }
        else
        {
            StopCoroutine(JumpAttack());
        }
    }
    IEnumerator JumpAttack()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector2.down * PlayerVerticalJumpForce/10, ForceMode2D.Impulse);

    }
    private void AirLinearDrag()
    {
        rb.drag = airlinearDrag;
    }
    private void Flip()
    {
    
        LookDirection = maincamera.ScreenToWorldPoint(Input.mousePosition);
        if (LookDirection.x < transform.position.x && lookingRight)
        {

            playerGraphic.Rotate(0f, 180f, 0f);
            playerOtherArm.Rotate(0f, 180f, 0f);
            lookingRight = false;

        }
        else if (LookDirection.x > transform.position.x && !lookingRight)
        {

            playerGraphic.Rotate(0f, 180f, 0f);
            playerOtherArm.Rotate(0f, 180f, 0f);

            lookingRight = true;

        }

        //// if the input is moving the player right and the player is facing left...
        //if (horizontal > 0 && !lookingright)
        //{
        //    // ... flip the player.
        //}
        //// otherwise if the input is moving the player left and the player is facing right...
        //else if (horizontal < 0 && lookingright)
        //{
        //    // ... flip the player.
        //    lookingright = !lookingright;
        //    transform.rotate(0f, 180f, 0f);
        //}


    }
    internal bool IsGrounded()
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
    private void Run(float runSpeed)
    {
        rb.AddForce(new Vector2(horizontal, 0f) * Acceleration);
        if (Mathf.Abs(rb.velocity.x) > runSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * runSpeed, rb.velocity.y);
        }
    }
    private void AirRun()
    {
        rb.AddForce(new Vector2(horizontal, 0f) * AirAcceleration);
        if (Mathf.Abs(rb.velocity.x) > Max_Horizontal_AirSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * RunSpeed, rb.velocity.y);
        }
    }

    private void Jump(float VerticalForce, float horizontalForce)
    {
        jumpRequested = false;


        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * VerticalForce, ForceMode2D.Impulse);
        rb.AddForce(horizontal * horizontalForce * Vector2.right, ForceMode2D.Impulse);

        hangTimeCounter = 0;
        jumpTimeCounter = 0;

        isJumping = true;
        jumpTimeCounter = jumpTime;
    }

    internal void TakeDamage(int value)
    {
        if (!isTakingDamage)
        {
            AudioManager.instance.Play(takeDamageSound);
            Health -= value;
            isTakingDamage = true;
            StartCoroutine(TakeDamageAnimation(0.05f));
            rb.velocity = Vector2.zero;

        }

    }
    IEnumerator TakeDamageAnimation(float timeInBetween)
    {
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(timeInBetween);
        playerSprite.color = Color.white;
        yield return new WaitForSeconds(timeInBetween);
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(timeInBetween);
        playerSprite.color = Color.white;
        isTakingDamage = false;
    }
    internal void Death()
    {
        Destroy(gameObject);
        string current;
        current = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(current);
        //Deathpanel
        //gameObject.SetActive(true);
    }


}
