using UnityEngine;
using System.Collections;
using NUnit.Framework;
// using System.Runtime.InteropServices;

public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer playerSR;
    public Rigidbody2D playerRB;
    public float moveSpeed;
    private Vector2 moveInput = Vector2.zero;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector2 dashDir = Vector2.down;
    public float attackTime;
    public float attackCooldown;
    private bool canAttack = true;
    private bool isAttacking = false;

    private Animator animator;
    private int lastDir = 2; // may use for animation
    // private int lastXDir = 1; // also for animation

    [Header("Idle Sprites")]
    public Sprite idleUp;
    public Sprite idleRight;
    public Sprite idleDown;
    public Sprite idleLeft;

    [Header("Dash Sprites")]
    public Sprite dashUp;
    public Sprite dashRight;
    public Sprite dashDown;
    public Sprite dashLeft;

    [Header("Attack Sprites")]
    public Sprite attackUp;
    public Sprite attackRight;
    public Sprite attackDown;
    public Sprite attackLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("facingDirection", lastDir);
    }
    
    int dirFromVect(Vector2 v)
    {
        return (Mathf.Abs(v.x) >= Mathf.Abs(v.y)) ? (v.x >= 0 ? 1 : 3) : (v.y >= 0 ? 0 : 2);
    }

    // void flipX(Vector2 v)
    // {
    //     if (Mathf.Abs(v.x) > 0.0001f) lastXDir = (v.x > 0f) ? +1 : -1;
    //     playerSR.flipX = lastXDir < 0; // true when facing left
    // }

    void SetIdle()
    {
        animator.SetBool("isWalking", false);
        if (animator.enabled) animator.enabled = false;
        int f = lastDir;
        switch (f)
        {
            case 0:
                playerSR.sprite = idleUp;
                break;
            case 1:
                playerSR.sprite = idleRight;
                // playerSR.flipX = lastXDir < 0;
                break;
            case 2:
                playerSR.sprite = idleDown;
                break;
            case 3:
                playerSR.sprite = idleLeft;
                break;
        }
        // Debug.Log($"idle sprite set to: {enemySR.sprite?.name} (facing={f})");
    }

    void SetWalking(int facing, Vector2 dir)
    {
        lastDir = facing;
        if (!animator.enabled) animator.enabled = true;
        animator.SetBool("isWalking", true);
        animator.SetInteger("facingDirection", facing);

        // if (facing == 1) flipX(dir);
    }

    void SetDash(Vector2 dir)
    {
        animator.SetBool("isWalking", false);
        if (animator.enabled) animator.enabled = false;

        int f = dirFromVect(dir);
        Debug.Log(f);
        lastDir = f;
        switch (f)
        {
            case 0:
                playerSR.sprite = dashUp;
                break;
            case 1:
                playerSR.sprite = dashRight;
                // playerSR.flipX = lastXDir < 0;
                break;
            case 2:
                playerSR.sprite = dashDown;
                break;
            case 3:
                playerSR.sprite = dashLeft;
                break;
        }
        // Debug.Log($"dash sprite set to: {enemySR.sprite?.name} (facing={f})");
    }

    void SetAttack(Vector2 dir)
    {
        animator.SetBool("isWalking", false);
        if (animator.enabled) animator.enabled = false;

        int f = dirFromVect(dir);
        Debug.Log(f);
        lastDir = f;
        switch (f)
        {
            case 0:
                playerSR.sprite = attackUp;
                break;
            case 1:
                playerSR.sprite = attackRight;
                // playerSR.flipX = lastXDir < 0;
                break;
            case 2:
                playerSR.sprite = attackDown;
                break;
            case 3:
                playerSR.sprite = attackLeft;
                break;
        }
        // Debug.Log($"attack sprite set to: {enemySR.sprite?.name} (facing={f})");
    }

    // Update is called once per frame
    void Update()
    {
        // stop  non-time-based controller inputs and processes when game is paused
        // if (SceneManager.Instance.gameIsPaused) return;

        float x = 0f, y = 0f;

        if (isDashing)
        {
            SetDash(dashDir);
        }
        else if (isAttacking)
        {
            SetAttack(dashDir);
        }
        else
        {
            //horizontal movement
            if (Input.GetKey(KeyCode.A))
            {
                x = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                x = 1f;
            }

            // vertical movement
            if (Input.GetKey(KeyCode.W))
            {
                y = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                y = -1f;
            }

            moveInput = new Vector2(x, y).normalized;

            if (moveInput != Vector2.zero)
            {
                dashDir = moveInput;
                SetWalking(dirFromVect(moveInput), moveInput);
            }
            else
            {
                animator.SetBool("isWalking", false);
                SetIdle();
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(DashTiming());
            }

            if (Input.GetKeyDown(KeyCode.Q) && canAttack)
            {
                StartCoroutine(AttackTiming());
            }
        }
    }

    // physics loop for movement
    void FixedUpdate()
    {
        if (isDashing)
        {
            playerRB.linearVelocity = dashDir * dashSpeed;
        }
        else
        {
            playerRB.linearVelocity = moveInput * moveSpeed;
        }
    }

    IEnumerator DashTiming()
    {
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator AttackTiming()
    {
        canAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    
    // public void SetMovementToZero() // used for plyer abilities/interactions that stop movement without time stop
    // {
    //     moveInput = Vector2.zero;
    //     playerRB.linearVelocity = Vector2.zero;
    //     isDashing = false;
    // }
}
