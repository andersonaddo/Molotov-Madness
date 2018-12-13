﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour
{

    public Vector2 targetPosition { get; private set; }

    public float minMaxAngle;
    [HideInInspector]public bool isFlipped;

    Quaternion rotateToPosition()
    {

        Vector3 pointTouched = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointTouched.x, pointTouched.y, 11));

        float deltaY = targetPosition.y - transform.position.y;
        float deltaX = targetPosition.x - transform.position.x;

        float angleToRotate = Mathf.Atan2(Mathf.Abs(deltaY), Mathf.Abs(deltaX));

        angleToRotate *= Mathf.Rad2Deg; //Converting to degrees

        //Adjusting the angle 
        if (deltaX < 0 && deltaY > 0) angleToRotate = 90 - angleToRotate;
        if (deltaX < 0 && deltaY < 0) angleToRotate = 90 + angleToRotate;
        if (deltaX > 0 && deltaY > 0) angleToRotate = 270 + angleToRotate;
        if (deltaX > 0 && deltaY < 0) angleToRotate = 270 - angleToRotate;
        if (deltaX == 0) angleToRotate = 0;

        angleToRotate -= 270;

        //hacky logic below
        isFlipped = GetComponentInParent<playerController>().transform.localScale.x < 0;

        if (isFlipped)
        {
            angleToRotate = Mathf.Clamp(angleToRotate + 180, -minMaxAngle, minMaxAngle);
            if (transform.localScale.x > 0) transform.localScale *= -1;
            return Quaternion.Euler(0, 0, 180 - angleToRotate);
        }
        else
        {
            angleToRotate = Mathf.Clamp(angleToRotate, -minMaxAngle, minMaxAngle);
            if (transform.localScale.x < 0) transform.localScale *= -1;
            return Quaternion.Euler(0, 0, angleToRotate);
        }
    }

    //This update is called after playerController's update, so isFlipped will be accurate
    void Update()
    {
        transform.localRotation = rotateToPosition();
    }
}
