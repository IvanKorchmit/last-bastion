using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Wall : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public float Health => health;
    public float MaxHealth => MaxHealth;
    private void Start()
    {
        maxHealth = health;
    }
    public void Damage(float d, GameObject owner)
    {
        health -= d;
        CheckHealth();
    }
    public void SetDurability(float v)
    {
        maxHealth = v;
    }
    private void CheckHealth()
    {
        if(health <= 0)
        {
            Collider2D coll = GetComponent<Collider2D>();
            coll.enabled = false;
            AstarPath.active.UpdateGraphs(coll.bounds);
            Destroy(gameObject);
        }
    }
}
