using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class molotov : MonoBehaviour
{

    public float angleOfLaunch, launchSpeed, angularVelocity;
    public string playerLayer;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        bool isFlipped = FindObjectOfType<playerController>().transform.localScale.x < 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angleOfLaunch * Mathf.Deg2Rad) * (isFlipped ? -1 : 1), Mathf.Sin(angleOfLaunch * Mathf.Deg2Rad) ) * launchSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * angularVelocity * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(playerLayer))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
