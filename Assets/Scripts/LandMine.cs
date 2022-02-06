using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour, IDamagable
{
    [SerializeField] private CircleCollider2D circle;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private AudioClip explosionSound;
    private float radius;
    public float Health => 0f;

    public float MaxHealth => 0f;
    void Start()
    {
        radius = circle.radius;
        Destroy(circle);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TimerUtils.AddTimer(1f,Blast);
        }
    }
    private void Blast()
    {
        RaycastHit2D[] res = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        foreach (var r in res)
        {
            if (r.collider != null)
            {
                if (r.collider.CompareTag("Enemy"))
                {
                    if (r.collider.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.Damage(40, gameObject);
                    }
                }
            }
        }
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        SoundManager.PlaySound(explosionSound, transform.position);
        Sectors.RemoveGameObject(gameObject, Sectors.PositionToSectorIndex(transform.position));
        Destroy(gameObject);

    }

    public void Damage(float d, GameObject owner)
    {
        Blast();
    }
}
