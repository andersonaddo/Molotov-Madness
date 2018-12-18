using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursor : MonoBehaviour
{

    public Vector3 targetPosition { get; private set; }
    public float gamepadMovementSpeed;
    public Transform player;


    // Update is called once per frame
    void Update()
    {
        if (gameManager.singletonInstance.usingGamePad)
        {
            Vector2 movementVector = new Vector2(InputManager.getGamepadRightX() * gamepadMovementSpeed * Time.deltaTime, InputManager.getGamepadRightY() * gamepadMovementSpeed * Time.deltaTime);
            movementVector = Vector2.ClampMagnitude(movementVector, gamepadMovementSpeed * Time.deltaTime);
            targetPosition += (Vector3) movementVector;
        }
        else
        {
            Vector3 pointTouched = Input.mousePosition;
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointTouched.x, pointTouched.y, 11));
        }

        //Clamping the position of the crosshairs to within the bounds of the main camera
        Vector3 pos = Camera.main.WorldToViewportPoint(targetPosition);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        targetPosition = Camera.main.ViewportToWorldPoint(pos);

        transform.position = targetPosition;
    }  

    public void flipLocalX(float currentSign)
    {
        if (currentSign == 0) return;
        float currentRelativeX = Mathf.Abs(transform.position.x - player.transform.position.x);
        targetPosition = new Vector3(player.position.x + currentRelativeX * currentSign, targetPosition.y, targetPosition.z);
    }
}
