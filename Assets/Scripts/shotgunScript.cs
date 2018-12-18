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

    public float shootingDistance, angleOfRange;
    [Range(1, 20)] public int numberOfRaysPerHalfSweep;
    public Transform barrelEnd;
    public LayerMask shootableLayers;

    public void shoot()
    {
        if (Time.time < nextShootTime) return;
        nextShootTime = Time.time + shootWaitTime;
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInParent<weaponManager>().shotgunAmmo--;

        //Launching empty bullet shell and giving it angular and translational velocity
        GameObject bullet = Instantiate(disgardedBullet, disgardedBulletPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().angularVelocity = disgardedBulletLaunchAngularVelosity;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.InverseTransformDirection(disgardedBulletLaunchDirection).normalized * disgardedBulletLaunchSpeed;
        if(GetComponent<aimingScript>().isFlipped) bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bullet.GetComponent<Rigidbody2D>().velocity.x, bullet.GetComponent<Rigidbody2D>().velocity.y);


        float angularSeparation = (angleOfRange / 2f) / numberOfRaysPerHalfSweep;
        Vector2 principalVector = barrelEnd.right * shootingDistance;
        List<GameObject> hitObjects = new List<GameObject>();

        for (int i = -numberOfRaysPerHalfSweep; i <= numberOfRaysPerHalfSweep; i++)
        {
            RaycastHit2D newHit = Physics2D.Raycast(barrelEnd.position, Quaternion.Euler(0, 0, angularSeparation * i) * principalVector, shootingDistance, shootableLayers);
            if (newHit.collider != null && !hitObjects.Contains(newHit.collider.gameObject))
            {
                GameObject hitObject = newHit.collider.gameObject;
                if (hitObject.GetComponent<IShotgunDamageable>() != null) hitObject.GetComponent<IShotgunDamageable>().reactToShot(Quaternion.Euler(0, 0, angularSeparation * i) * principalVector);
                hitObjects.Add(newHit.collider.gameObject);
            }

            if (newHit.collider == null) Debug.DrawRay(barrelEnd.position, Quaternion.Euler(0, 0, angularSeparation * i) * principalVector, Color.red, 0.75f);
            else Debug.DrawRay(barrelEnd.position, newHit.point - (Vector2)barrelEnd.position, Color.red, 0.75f);
        }
    }
}
