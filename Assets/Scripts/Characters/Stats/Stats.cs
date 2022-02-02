using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LastBastion.Waves;
using LastBastion.Economy;
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
    private string lastDamager;
    private GameObject lastDamagerGO;
    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public float MaxHealth => maxHealth;
    [SerializeField]
    private int costOnKill;
    private bool isOnFire;
    [SerializeField] ParticleSystem fire;
    public void Damage(float d, GameObject owner)
    {
        d = Random.Range(0, d);
        if (d <= 0) return;
        float target = 0.1f;
        float randValue = Random.value;
        if (randValue < (1f-target))
        {
            d *= 1.5f;
        }
        health -= d;
        lastDamager = owner != null ? owner.tag : null;
        lastDamagerGO = owner;
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
            if (lastDamager != null && lastDamager == "Player")
            {
                ShopUtils.GainMoney(costOnKill);
                System.Array vals = System.Enum.GetValues(typeof(ShopUtils.ResourceType));
                ShopUtils.ResourceType type = (ShopUtils.ResourceType)vals.GetValue(Random.Range(0, vals.Length));
                ShopUtils.GainResource(type, Random.Range(0, 4));
            }
            TimerUtils.AddTimer(0.02f, () => WavesUtils.CheckRemainings());
        }
        GetComponent<IUnsub>().UnsubAll();
        Destroy(gameObject);
    }

    private void Start()
    {
        initMeleeDamage = meleeDamage;
        isUnit = CompareTag("Player");
        maxHealth = health;
        fire.Stop();
    }
    public void SetOnFire()
    {
        isOnFire = true;
        TimerUtils.AddTimer(10, RemoveFire);
        fire.Play();
    }
    private void RemoveFire()
    {
        isOnFire = false;
        fire.Stop();
    }
    private void Update()
    {
        CheckHealth();
        if (isOnFire)
        {
            TimerUtils.AddTimer(1, FireDamage);
        }
    }
    private void FireDamage()
    {
        Damage(Random.Range(3f, 6f), lastDamagerGO);
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
