using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public float HealthMax => healthMax;
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
                TimerUtils.AddTimer(0.02f, WavesUtils.CheckRemainings); 
                ShopUtils.GainMoney(100);
            }
            gameObject.SetActive(false);

        }
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
