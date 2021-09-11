    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    public float Health => health;

    public void Damage(float d, GameObject owner)
    {
        health -= d;
        CheckHealth();
    }
    private void CheckHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
