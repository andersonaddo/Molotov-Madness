using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float movementSpeed;
    [HideInInspector]public bool isGrounded;
    bool canMove = true;
    bool isJumping, isWalking;
    public float jumpDelay; //How mong movement is disabled after a jump
    public float jumpForce;

    Rigidbody2D myRb;
    Animator animator;
    public aimingScript shotgun;

    //Variables for flipping
    float previousShotgunTargetRelativeXSign, previousHorizontalInputSign;


    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        determineFaceDirection();

        if (InputManager.getJumpInput() && !isJumping)
        {
            StartCoroutine("Jump");
        }

        if (InputManager.getHorizontalAxis() != 0 && canMove)
        {
            //maintain velocity
            transform.position = (movementSpeed * Vector3.right * Time.deltaTime * InputManager.getHorizontalAxis() + transform.position);

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

    //This allows the shotgun to also influence where the playre if facing
    void determineFaceDirection()
    {
        if (InputManager.getHorizontalAxis() != 0)
        {
            float currentSign = Mathf.Sign(InputManager.getHorizontalAxis());
            if (previousHorizontalInputSign != currentSign)
            {
                transform.localScale = new Vector3(InputManager.getHorizontalAxis() * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                shotgun.crosshairs.flipLocalX(currentSign);
            }
            previousHorizontalInputSign = currentSign;
        }
        else if (GetComponent<weaponManager>().equippedWeapon == weaponManager.weapon.shotgun) //if the player isn't pressing the movement buttons and is aiming
        {
            float shotgunTargetRelativeXSign = Mathf.Sign(shotgun.crosshairs.targetPosition.x - transform.position.x);
            if (previousShotgunTargetRelativeXSign != shotgunTargetRelativeXSign)
            {
                transform.localScale = new Vector3(shotgunTargetRelativeXSign * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                shotgun.crosshairs.flipLocalX(shotgunTargetRelativeXSign);
            }
            previousShotgunTargetRelativeXSign = shotgunTargetRelativeXSign;
        }
    }



}
