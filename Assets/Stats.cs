using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IDamagable
{
    private float health;
    private float armor;
    private float meleeDamage;
    private float rangedDamage;
    public float Health => health;
    public float Armor => armor;
    public float MeleeDamage => meleeDamage;
    public float RangedDamage => rangedDamage;
    public void Damage(float d)
    {
        health -= d;
        checkHealth();
    }
    private void checkHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject); // Use this as the placeholder.
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
