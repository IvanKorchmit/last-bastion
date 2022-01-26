using UnityEngine;
public abstract class GameEvent : ScriptableObject
{
    [SerializeField] private new string name;
    public string Name => name;
    public abstract void Launch();
}
