using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.Waves;
public class ColonySystem : MonoBehaviour
{
    public static event System.Action OnEnemyEnter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            HumanResourcesUtils.IncreaseChaos(0.05f);
            Destroy(collision.gameObject);
            TimerUtils.AddTimer(0.03f,()=>WavesUtils.CheckRemainings());
            OnEnemyEnter?.Invoke();
        }   
    }
}
