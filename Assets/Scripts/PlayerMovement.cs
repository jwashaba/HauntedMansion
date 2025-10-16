using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer playerSR;
    public Rigidbody2D playerRB;
    public float moveSpeed;
    private Vector2 moveInput = Vector2.zero;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    public bool canDash = true;
    private bool isDashing = false;
    private Vector2 dashDir = Vector2.down;
    // private int lastDir = 2; // may use for animation
    // private int lastXDir = -1; // also for animation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set up animator stuff here
    }
    
    // int dirFromVect(Vector2 v)
    // {
    //     return (Mathf.Abs(v.x) >= Mathf.Abs(v.y)) ? 1 : (v.y >= 0 ? 0 : 2);
    // }

    // void flipX(Vector2 v)
    // {
    //     if (Mathf.Abs(v.x) > 0.0001f) lastXDir = (v.x > 0f) ? +1 : -1;
    //     playerSR.flipX = lastXDir > 0; // true when facing right
    // }

    // Update is called once per frame
    void Update()
    {
        // stop  non-time-based controller inputs and processes when game is paused
        // if (SceneManager.Instance.gameIsPaused) return;

        float x = 0f, y = 0f;

        if (!isDashing)
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
                // int facing = dirFromVect(moveInput); // animation
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(DashTiming());
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
    
    // public void SetMovementToZero()
    // {
    //     moveInput = Vector2.zero;
    //     playerRB.linearVelocity = Vector2.zero;
    //     isDashing = false;
    // }
}
