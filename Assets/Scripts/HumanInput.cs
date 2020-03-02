using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : InputController
{
    public override float GetAcceration()
    {
        return Input.GetAxis("Acceleration");
    }

    public override float GetSteering()
    {
        return Input.GetAxis("Steering");
    }
}
