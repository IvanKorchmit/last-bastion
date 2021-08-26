using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if(collision.TryGetComponent(out IDamagable damage))
            {
                damage.Damage(5);
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
