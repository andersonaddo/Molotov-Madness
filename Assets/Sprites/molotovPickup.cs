using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class molotovPickup : MonoBehaviour
{
    public string playerLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
        {
            other.GetComponentInParent<weaponManager>().numberOfMolotovs++;
            Destroy(gameObject);
        }
    }
}
