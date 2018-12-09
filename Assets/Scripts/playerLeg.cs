using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLeg : MonoBehaviour
{
    public string targetLayer;

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            GetComponentInParent<playerController>().isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            GetComponentInParent<playerController>().isGrounded = false;
        }
    }
}
