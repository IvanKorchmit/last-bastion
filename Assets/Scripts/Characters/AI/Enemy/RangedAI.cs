using UnityEngine;

public class RangedAI : AIBase
{
    [SerializeField] private WeaponBase weapon;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
        if (target != null)
        {
            weapon.Use(gameObject, target.transform);
        }
    }
}
