﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinoScript : MonoBehaviour
{

    Animator animator;

    //Patrolling Variables
    public LayerMask layersThatBlockPatrolRaycast;   
    public float maxPatrolDistance, patrolSpeed;
    //Wall buffer is needed because if the dino is patrolling to a location very close to a wall, it might never actially 
    //Alighn its center with it due to its own collider blocking it.
    float radius, wallBuffer;
    float patrolTargetX;
    public float patrolDestinationWaitTime; //The waiting time when the dino reaches a patrol destination

    //Charging variables
    public LayerMask playerLayerMask;
    float nextCheckTime, chargeTargetX, nextSearchTime;
    public float chargeSpeed, alertSampleRate;
    Collider2D playerCollider;

    bool goingLeft;

    public enum dinoState { patrol, charging, hurt};
    public dinoState currentState { get; private set; }
    public dinoState lastState { get; private set; }
    public isGroundedRaycaster goundedChecker;


    void Start()
    {
        radius = GetComponentInChildren<CircleCollider2D>().radius * transform.localScale.x;
        animator = GetComponent<Animator>();
        goundedChecker = GetComponent<isGroundedRaycaster>();

        wallBuffer = GetComponent<BoxCollider2D>().bounds.extents.x * transform.lossyScale.x + 0.03f;

        currentState = dinoState.patrol;
        lastState = currentState;
        getNewPatrolTarget();

        StartCoroutine("searchAndCharge");
        StartCoroutine("patrol");

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(new Vector2(patrolTargetX, transform.position.y), Vector3.one/3);
    //}

    public void changeState(dinoState newState)
    {
        lastState = currentState;
        currentState = newState;
        if (newState == dinoState.patrol) getNewPatrolTarget();
    }

    void getNewPatrolTarget()
    {
        goingLeft = Random.Range(0, 2) == 1;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.right * (goingLeft ? -1 : 1), maxPatrolDistance, layersThatBlockPatrolRaycast);
        patrolTargetX = 0;
        float xLimit = 0;

        if (raycast.collider == null)
            xLimit = (transform.position.x + (goingLeft ? -maxPatrolDistance : maxPatrolDistance));
        else xLimit = raycast.point.x;

        xLimit += (goingLeft ? wallBuffer : -wallBuffer);
        if (goingLeft) patrolTargetX = Random.Range(xLimit, transform.position.x);
        else patrolTargetX = Random.Range(transform.position.x, xLimit);
        goingLeft = patrolTargetX < transform.position.x; //Sometimes adding the wallBuffer can make targets that are originally left or right change to right or left.
        GetComponent<SpriteRenderer>().flipX = goingLeft;
    }


    IEnumerator patrol()
    {
        while (true)
        {
            yield return null;
            if (!goundedChecker.isGrounded) continue;
            if (currentState != dinoState.patrol) continue;
            animator.SetBool("isWalking", true);
            transform.Translate(Vector2.right * (goingLeft ? -1 : 1) * patrolSpeed * Time.deltaTime); //ideally I should be using rigidbody.moveposition, but it can sometimes stop movement for no reason
            if ((goingLeft && transform.position.x <= patrolTargetX) || (!goingLeft && transform.position.x >= patrolTargetX))
            {
                animator.SetBool("isWalking", false);
                yield return new WaitForSeconds(patrolDestinationWaitTime);
                getNewPatrolTarget();
            }
        }
    }


    IEnumerator searchAndCharge()
    {
        while (true)
        {
            yield return null;
            if (!goundedChecker.isGrounded) continue;
            if (currentState == dinoState.hurt) continue;
            //Start charging immediately if you you notice him for first time
            if (playerCollider == null)
            {
                playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
                if (playerCollider)
                {
                    chargeTargetX = playerCollider.transform.position.x;
                    goingLeft = chargeTargetX < transform.position.x;
                    GetComponent<SpriteRenderer>().flipX = goingLeft;
                    changeState(dinoState.charging);
                    nextCheckTime = Time.time + alertSampleRate;
                }
            }
            else if (Time.time > nextCheckTime) //Check periodically if the player is still in range
            {
                playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
                if (playerCollider)
                {
                    chargeTargetX = playerCollider.transform.position.x;
                    goingLeft = chargeTargetX < transform.position.x;
                    GetComponent<SpriteRenderer>().flipX = goingLeft;
                    nextCheckTime = Time.time + alertSampleRate;
                }else
                {
                    changeState(dinoState.patrol);
                    playerCollider = null;
                }
            }

            if (currentState == dinoState.charging)
            {
                transform.Translate(Vector2.right * (goingLeft ? -1 : 1) * chargeSpeed * Time.deltaTime); //ideally I should be using rigidbody.moveposition, but it can sometimes stop movement for no reason
            }

            animator.SetBool("isCharging", currentState == dinoState.charging);
        }
    }
}
