using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float movementSpeed;
    [HideInInspector]public bool isGrounded;
    bool canMove = true;
    bool isJumping, isWalking;
    public float jumpDelay;
    public float jumpForce;

    Rigidbody2D myRb;
    Animator animator;
      
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine("Jump");
        }

        if (Input.GetAxisRaw("Horizontal") != 0 && canMove)
        {
            //maintain velocity
            transform.position = (movementSpeed * Vector3.right * Time.deltaTime * Input.GetAxisRaw("Horizontal") + transform.position);

            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal") * Mathf.Abs(transform.localScale.x) , transform.localScale.y, transform.localScale.z);
            if (!animator.GetBool("isWalking") && isGrounded) animator.SetBool("isWalking", true);
            else if (animator.GetBool("isWalking") && !isGrounded) animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        
    }

    IEnumerator Jump()
    {
        canMove = false;
        isJumping = true;
        animator.SetTrigger("jump");
        yield return new WaitForSeconds(jumpDelay);
        myRb.AddForce(Vector2.up * jumpForce);
        canMove = true;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(()=> isGrounded);
        isJumping = false;
        animator.SetTrigger("landed");
    }
}
