using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private RangeFinder range;
    public Weapon @Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }

    }
    private void Start()
    {
        range = GetComponentInChildren<RangeFinder>();
    }
    private void Update()
    {
        if (weapon is Firearm f)
        {
            if (range.ClosestTarget != null)
            {
                TimerUtils.AddTimer(f.Cooldown, Attack);
            }
        }
    }
    public void Attack()
    {
        weapon.Use(gameObject, range.ClosestTarget);
    }
}
