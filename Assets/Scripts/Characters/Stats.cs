using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LastBastion.Waves;
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
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image armorBar;
    private bool isUnit;
    private float healthMax;
    private float armorMax = 10f;
    private GameObject lastDamager;
    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public float HealthMax => healthMax;
    public void Damage(float d, GameObject owner)
    {
        health -= d;
        lastDamager = owner;
    }
    private void CheckHealth()
    {
        if(health <= 0)
        {
            Death();

        }
    }

    private void Death()
    {
        if (CompareTag("Enemy"))
        {
            if (lastDamager != null && lastDamager.CompareTag("Player"))
            {
                ShopUtils.GainMoney(100);
            }
            TimerUtils.AddTimer(0.02f, WavesUtils.CheckRemainings);
        }
        GetComponent<IUnsub>().UnsubAll();
        Destroy(gameObject);
    }

    private void Start()
    {
        isUnit = CompareTag("Player");
        healthMax = health;
    }
    private void Update()
    {
        CheckHealth();
    }
    private void OnGUI()
    {
        if (isUnit)
        {
            healthBar.fillAmount = health / healthMax;
            armorBar.fillAmount = armor / armorMax;
        }
    }
}
