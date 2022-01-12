using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonySystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            HumanResourcesUtils.IncreaseChaos(0.1f);
            Destroy(collision.gameObject);
            TimerUtils.AddTimer(0.02f,()=>WavesUtils.CheckRemainings());
        }   
    }
}
