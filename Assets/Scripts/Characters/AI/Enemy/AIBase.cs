using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
[RequireComponent(typeof(Stats))]
public class AIBase : MonoBehaviour
{
    private Vector2 moveDirection;
    private Stats stats;
    private Seeker seeker;
    private Path path;
    private RangeFinder range;
    [SerializeField] private float speed;
    [SerializeField] private bool isAttacking;
    protected IDamagable target;
    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
        seeker = GetComponent<Seeker>();
        range = GetComponentInChildren<RangeFinder>();

    }

    protected virtual void Update()
    {
        MoveAlong();
        transform.Translate(moveDirection * speed * Time.deltaTime);
        TimerUtils.AddTimer(0.5f, Attack);
        UpdateTarget();
        
    }
    private void UpdateTarget()
    {
        if (target != null && range.ClosestTarget != null && target.transform != range.ClosestTarget)
        {
            if (range.ClosestTarget == null)
            {
                target = null;
                isAttacking = false;

            }
            else
            {
                target = range.ClosestTarget.GetComponent<IDamagable>();
            }

        }
        else if (range.ClosestTarget == null)
        {
            target = null;
            isAttacking = false;
        }
    }
    private void OnPathCalculated(Path p)
    {
        if(!p.error)
        {
            path = p;
        }
    }
    
    private void MoveAlong()
    {
        if (isAttacking)
        {
            moveDirection = Vector2.zero;
            return;
        }
        else if (path == null)
        {
            if (range.ClosestTarget == null)
            {
                seeker.StartPath(transform.position,
                    GameObject.Find(WavesUtils.COLONY_PATH).transform.position, OnPathCalculated);
            }
            else
            {
                seeker.StartPath(transform.position,
                    range.ClosestTarget.position, OnPathCalculated);
            }
        }
        if (path != null)
        {
            if(range.ClosestTarget != null)
            {
                seeker.StartPath(transform.position,
                    range.ClosestTarget.position, OnPathCalculated);
                isAttacking = true;
                target = range.ClosestTarget.GetComponent<IDamagable>();
            }
            moveDirection = ((Vector3)path.path[0].position - transform.position).normalized;
            if (Vector2.Distance(transform.position, (Vector3)path.path[0].position) <= 0.5f)
            {
                if (path.path.Count - 1 > 0)
                {
                    path.path.RemoveAt(0);
                }
                else
                {
                    path = null;
                }
            }
        }
    }
    protected virtual void Attack()
    {
        if(isAttacking)
        {
            if (target != null)
            {
                target.Damage(stats.MeleeDamage, gameObject);
                if(target.Health <= 0)
                {
                    target = null;
                }
            }
            else
            {
                
                isAttacking = false;
                MoveAlong();
                return;
            }
        }
    }
}

public interface IDamagable
{
    void Damage(float d, GameObject owner);
    float Health { get; }
    Transform transform { get; }
}   