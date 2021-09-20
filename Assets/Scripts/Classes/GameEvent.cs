﻿using UnityEngine;
public class GameEvent : ScriptableObject
{
    public virtual void Launch()
    {
        Debug.Log("Event launched, nothing happened since it is ran from the base class");
    }
    public virtual void End()
    {
        Debug.Log("Event has ended, nothing happened since it is ran from the base class");
    }
}