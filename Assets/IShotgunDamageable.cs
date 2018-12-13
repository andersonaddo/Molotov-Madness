using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any object that should respond to a shotgun shot should impliment this interface
/// </summary>
public interface IShotgunDamageable
{
    void reactToShot(Vector2 incomingShotTrajectory);
}
