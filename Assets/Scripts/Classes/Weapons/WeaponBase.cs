using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public abstract void Use(Transform owner, Transform target);

}
