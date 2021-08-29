using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    [SerializeField] private MinerAI miner;
    private bool isOccupied(MinerAI miner)
    {
        if (this.miner == null || this.miner == miner)
        {
            return false;
        }
        return true;
    }
    
    public float Health => health;
    public bool SetMiner(MinerAI miner)
    {
        if (miner != null)
        {
            if (!isOccupied(miner))
            {
                this.miner = miner;
                return true;
            }
        }
        return false;
    }
    public void Damage(float d)
    {
        health -= d;
        miner.AddQuantity((int)d);
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (health <= 0)
        {
            if (miner.OreTarget == this)
            {
                miner.OreTarget = null;
            }
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
