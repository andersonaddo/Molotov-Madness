using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinoDamageInstructions : MonoBehaviour, IDamageInstructions, IShotgunDamageable
{
    public float damageForceMagnitude, postDamageWaitTime;
    public Vector3 damageForgeDireciton;
    dinoScript dinoScript;
    Animator animator;
    Rigidbody2D myRB;

    float restorationTime;

    public int shotgunDamage;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dinoScript = GetComponent<dinoScript>();
    }

    void Update()
    {
        if (dinoScript.currentState == dinoScript.dinoState.hurt)
        {
            if (Time.time >= restorationTime) dinoScript.changeState(dinoScript.dinoState.patrol);
        }
    }

    public void executeDamageInstructions(int damage)
    {
        restorationTime = Time.time + postDamageWaitTime;
        dinoScript.changeState(dinoScript.dinoState.hurt);
        animator.SetTrigger("hurt");
    }

    //Launches the dino in the air a bit
    public void reactToShot(Vector2 incomingShotTrajectory)
    {
        GetComponent<health>().incrementHealthBy(-shotgunDamage);
        Vector2 launchVector = damageForgeDireciton.normalized * damageForceMagnitude;
        launchVector.x *= (incomingShotTrajectory.x < 0) ? -1 : 1;
        myRB.AddForce(launchVector, ForceMode2D.Impulse);
    }
}
