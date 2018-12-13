using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGroundedRaycaster : MonoBehaviour
{

    public bool isGrounded { get; private set; }
    public float raycastLength;
    public bool drawRaycastLine;
    public LayerMask groundLayer;

    void Update()
    {
        if (drawRaycastLine) Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastLength, Color.green);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector3.down, raycastLength, groundLayer).collider != null;
    }
}
