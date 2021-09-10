using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public virtual void Use(GameObject owner, Transform target)
    {
        Debug.Log("Pew pew");
    }
}
