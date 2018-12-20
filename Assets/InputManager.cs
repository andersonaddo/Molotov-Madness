using UnityEngine;

public static class InputManager
{
    public static float getHorizontalAxis()
    {
        if (Input.GetAxisRaw("Keyboard Horizontal") != 0) return Input.GetAxisRaw("Keyboard Horizontal");
        else if (Input.GetAxisRaw("PS4 Horizontal") != 0)
        {
            if (Input.GetAxisRaw("PS4 Horizontal") > 0.2f) return 1;
            else if (Input.GetAxisRaw("PS4 Horizontal") < -0.2f) return -1;
            return 0;
        }
        else return 0;
    }

    //The X button or the space bar
    public static bool getJumpInput()
    {
        return Input.GetButtonDown("Keyboard and PS4 Jump");
    }

    //Square button or the z key
    public static bool getWeaponSwitchInput()
    {
        return Input.GetButtonDown("Keyboard and PS4 Switch Weapons");
    }

    //R2 or left mouse button
    public static bool primaryInputDown()
    {
        return Input.GetButtonDown("Keyboard and PS4 Primary");
    }

    //R1 or right mouse button
    public static bool secondaryInputDown()
    {
        return Input.GetButtonDown("Keyboard and PS4 Secondary");
    }

    public static float getGamepadRightX()
    {
        return Input.GetAxis("PS4 Right Analog Horizontal");
    }

    public static float getGamepadRightY()
    {
        return -Input.GetAxis("PS4 Right Analog Vertical"); //Unity inverts it for some reason
    }

    public static bool getResetInput()
    {
        return Input.GetButtonDown("Keyboard and PS4 reset");
    }
}
