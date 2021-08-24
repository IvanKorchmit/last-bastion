using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Stats))]
public class AIBase : MonoBehaviour
{
    private Vector2 moveDirection;
    private Stats stats;
    private readonly Vector2 DEF_DIR = Vector2.left;
    [SerializeField] private float speed;
    private int obstLayer;
    private bool isAttacking;
    private IDamagable target;
    private float timer;
    private float cooldown = 1.5f;
    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
        obstLayer = LayerMask.NameToLayer("Obstacles");
    }

    protected virtual void Update()
    {
        CheckObstacles();
        transform.Translate(moveDirection * speed * Time.deltaTime);
        if(timer >= cooldown)
        {
            timer -= timer;
            Attack();
        }
    }
    private void CheckObstacles()
    {
        Vector2 pos = (Vector2)transform.position + new Vector2(0.25f,0);
        RaycastHit2D ray = Physics2D.BoxCast(pos, new Vector2(0.5f,1), 0, transform.right, 0);
        if (ray.collider != null && ray.collider.gameObject != gameObject)
        {
            if (moveDirection == DEF_DIR && obstLayer == ray.collider.gameObject.layer)
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
            else if (ray.collider.gameObject.TryGetComponent(out IDamagable damage))
            {
                moveDirection = Vector2.zero;
            }
        }
        else
        {
            if (moveDirection != DEF_DIR)
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
            }
            else
            {
                isAttacking = false;
                return;
            }
        }
    }
}
interface IDamagable
{
    void Damage(float d);
}