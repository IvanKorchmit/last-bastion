using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Stats))]
public class AIBase : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleCheckMask;
    private Vector2 moveDirection;
    private Stats stats;
    private readonly Vector2 DEF_DIR = Vector2.left;
    [SerializeField] private float speed;
    private int obstLayer;
    [SerializeField] private bool isAttacking;
    private IDamagable target;
    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
        obstLayer = LayerMask.NameToLayer("Obstacles");
        
    }

    protected virtual void Update()
    {
        CheckObstacles();
        transform.Translate(moveDirection * speed * Time.deltaTime);
        TimerUtils.AddTimer(0.5f, Attack);
    }
    private void CheckObstacles()
    {
        if (isAttacking) return;

        Vector2 pos = transform.position;
        Vector2 size = new Vector2(0.25f, 1);
        int l = obstacleCheckMask;
        RaycastHit2D ray = Physics2D.BoxCast(pos, size, 0, DEF_DIR, 0.5f, l);
        if (ray.collider != null && ray.collider.gameObject != gameObject)
        {
            if (obstLayer == ray.collider.gameObject.layer)
            {
                if (moveDirection == DEF_DIR)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 1:
                            moveDirection = Vector2.up;
                            break;
                        case 0:
                            moveDirection = Vector2.down;
                            break;
                    }
                }
            }
            else if (ray.collider.gameObject.TryGetComponent(out IDamagable damage))
            {
                moveDirection = Vector2.zero;
                isAttacking = true;
                target = damage;
            }
        }
        else
        {
            if (moveDirection != DEF_DIR && !isAttacking)
            {
                moveDirection = DEF_DIR;
            }
        }
    }
    private void Attack()
    {
        if(isAttacking)
        {
            if (target != null)
            {
                target.Damage(stats.MeleeDamage);
                if(target.Health <= 0)
                {
                    target = null;
                }
            }
            else
            {
                
                isAttacking = false;
                CheckObstacles();
                return;
            }
        }
    }
}
interface IDamagable
{
    void Damage(float d);
    float Health { get; }
}