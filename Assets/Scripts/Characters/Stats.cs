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
    private float initMeleeDamage;
    [SerializeField]
    private float rangedDamage;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image armorBar;
    private bool isUnit;
    private float maxHealth;
    private float armorMax = 10f;
    private GameObject lastDamager;
    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public float MaxHealth => maxHealth;
    [SerializeField]
    private int costOnKill;
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
    public void IncreaseMeleeDamage(float value)
    {
        meleeDamage = initMeleeDamage * value;
    }
    private void Death()
    {
        if (CompareTag("Enemy"))
        {
            if (lastDamager != null && lastDamager.CompareTag("Player"))
            {
                ShopUtils.GainMoney(costOnKill);
            }
            TimerUtils.AddTimer(0.02f, WavesUtils.CheckRemainings);
        }
        GetComponent<IUnsub>().UnsubAll();
        Destroy(gameObject);
    }

    private void Start()
    {
        initMeleeDamage = meleeDamage;
        isUnit = CompareTag("Player");
        maxHealth = health;
    }
    private void Update()
    {
        CheckHealth();
    }
    private void OnGUI()
    {
        if (isUnit)
        {
            healthBar.fillAmount = health / maxHealth;
            armorBar.fillAmount = armor / armorMax;
        }
    }
}
