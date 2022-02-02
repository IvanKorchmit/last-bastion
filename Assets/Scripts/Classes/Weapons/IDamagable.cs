using UnityEngine;

public interface IDamagable
{
    void Damage(float d, GameObject owner);
    float Health { get; }
    float MaxHealth { get; }
    Transform transform { get; }
}   