using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Stats stats))
        {
            stats.SetOnFire();
            stats.Damage(2f/100f, gameObject);
        }
    }
}
