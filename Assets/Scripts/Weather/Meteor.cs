using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private CircleCollider2D circle;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private AudioClip explosionSound;
    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }
    public void Blast()
    {
        RaycastHit2D[] res = Physics2D.CircleCastAll(transform.position, circle.radius, Vector2.zero);
        foreach (var r in res)
        {
            if (r.collider != null)
            {
                if (r.collider.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage(40, gameObject);
                }
            }
        }
        ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps.Stop();
        ps.transform.SetParent(null);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        SoundManager.PlaySound(explosionSound, transform.position);
        Destroy(gameObject);
    }
}
