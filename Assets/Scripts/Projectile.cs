using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool isEnemy;
    public void Initialize(float damage, bool isEnemy)
    {
        this.damage = damage;
        this.isEnemy = isEnemy;
    }
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(isEnemy ? "Player" : "Enemy") || (isEnemy ? collision.CompareTag("Wall") : false))
        {
            if(collision.TryGetComponent(out IDamagable damage))
            {
                damage.Damage(this.damage, null);
                Transform trail = transform.Find("Trail");
                if (trail != null)
                {
                    trail.SetParent(null);
                }
                Destroy(gameObject);
            }
        }
    }
}
