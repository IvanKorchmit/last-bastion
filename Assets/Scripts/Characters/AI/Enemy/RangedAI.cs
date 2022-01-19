using UnityEngine;

public class RangedAI : AIBase
{
    [SerializeField] protected Transform shootPoint;
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
            weapon.Use(shootPoint, target.transform);
        }
    }
}
