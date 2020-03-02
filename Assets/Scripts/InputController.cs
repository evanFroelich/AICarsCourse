using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public virtual float GetAcceration() { return 0; }
    public virtual float GetSteering() { return 0; }
}
