using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 250;
    [SerializeField] private float jumpForce = 400;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDead = false;

    private float horizontal;
    private string currentAnimName;
    private int coins = 0;
    public Vector3 checkPoint;

    // Start is called before the first frame update
    void Start()
    {
        CheckPoint();
        OnInit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        // -1 -> 0 -> 1
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isDead)
        {
            return;
        }
        
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            if (Input.GetKeyDown(KeyCode.J) && isGrounded)
            {
                Attack();
            }
            
            if (Input.GetKeyDown(KeyCode.K) && isGrounded)
            {
                Throw();
            }
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);

            //transform.localScale = new Vector3(horizontal, 1, 1);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    public void OnInit()
    {
        isDead = false;
        isAttack = false;

        transform.position = checkPoint;
        ChangeAnim("idle");
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, UnityEngine.Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{ 
        //    return false; 
        //}
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {
        ChangeAnim("ilde");
        isAttack = false;
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName) 
        { 
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }

    internal void CheckPoint()
    {
        checkPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coins")
        {
            coins++;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeadZone")
        {
            isDead = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 0.5f);
        }
    }
}
