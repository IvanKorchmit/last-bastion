using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public abstract class ResearchUpgrade : ScriptableObject
{
    [SerializeField] protected int level;
    public int Level => level;
    public abstract void OnUpgrade();

}

