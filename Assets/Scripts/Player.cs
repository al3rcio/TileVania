using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 4f;
    float climbSpeed = 3f;
    float jumpForce = 150f;

    Animator animator;
    Rigidbody2D rb;
    Collider2D col2d;

   
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveX();
        Jump();
        ClimbLadder();

    }

    void PlayerMoveX()
    {
        if (Input.GetButton("Horizontal"))
        {
            animator.SetBool("isRunning", true);
            float horizontalMove = Input.GetAxis("Horizontal");
            float deltaPosX = speed * Time.deltaTime * horizontalMove;
            float newPosX = deltaPosX + transform.position.x;
            transform.position = new Vector2(newPosX, transform.position.y);
            if (horizontalMove < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            //rb.velocity = new Vector2(speed * horizontalMove, rb.velocity.y);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!col2d.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

            if (col2d.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                rb.AddForce(transform.up * jumpForce);
            }
        }
    }

    private void ClimbLadder()
    {
        if (!col2d.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        { 
            rb.WakeUp();
            animator.SetBool("ClimbingPos", false);
            animator.SetBool("isClimbing", false);
            return;  
        }

        if (col2d.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            animator.SetBool("ClimbingPos", true);
           
            rb.Sleep();
            if (Input.GetButton("Vertical"))
            {
                //animator.SetBool("ClimbingPos", false);

                StartCoroutine(WaitAndClimb());
            }
            else
            {
                animator.SetBool("ClimbingPos", true);
                animator.SetBool("isClimbing", false);             
            }
        }  
    }

    IEnumerator WaitAndClimb()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isClimbing", true);
        float verticalMove = Input.GetAxis("Vertical");
        float deltaPosY = climbSpeed * verticalMove;
        rb.velocity = new Vector2(rb.velocity.x, deltaPosY);
    }


}
