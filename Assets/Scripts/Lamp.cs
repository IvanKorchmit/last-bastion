using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    private float maxHealth;
    public float Health => health;

    public float MaxHealth => maxHealth;
    public void Damage(float d, GameObject owner)
    {
        health -= d;
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (health < 0)
        {
            Sectors.RemoveGameObject(gameObject, Sectors.PositionToSectorIndex(transform.position));
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        maxHealth = health;
    }
    
}
