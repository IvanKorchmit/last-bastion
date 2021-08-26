using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float armor;
    [SerializeField]
    private float meleeDamage;
    [SerializeField]
    private float rangedDamage;
    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public void Damage(float d)
    {
        health -= d;
    }
    private void CheckHealth()
    {
        if(health <= 0)
        {
            if(CompareTag("Enemy"))
            {
                ShopUtils.GainMoney(100);
            }
            Destroy(gameObject); // Use this as the placeholder.

        }
    }
    private void Update()
    {
        CheckHealth();
    }
}
