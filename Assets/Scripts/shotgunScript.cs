using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunScript : MonoBehaviour
{
    public float shootWaitTime;
    float nextShootTime;
    public GameObject disgardedBullet;
    public Vector2 disgardedBulletLaunchDirection;
    public float disgardedBulletLaunchAngularVelosity, disgardedBulletLaunchSpeed;
    public Transform disgardedBulletPosition;

    public void shoot()
    {
        if (Time.time < nextShootTime) return;
        nextShootTime = Time.time + shootWaitTime;
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInParent<weaponManager>().shotgunAmmo--;
        GameObject bullet = Instantiate(disgardedBullet, disgardedBulletPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().angularVelocity = disgardedBulletLaunchAngularVelosity;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.InverseTransformDirection(disgardedBulletLaunchDirection).normalized * disgardedBulletLaunchSpeed;
        if(GetComponent<followMouse>().isFlipped) bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bullet.GetComponent<Rigidbody2D>().velocity.x, bullet.GetComponent<Rigidbody2D>().velocity.y);
    }
}
