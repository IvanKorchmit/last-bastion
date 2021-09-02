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
    private IDamagable target;
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
        if (target != null && target.transform != range.ClosestTarget)
        {
            if (range.ClosestTarget == null)
            {
                target = null;
            }
            else
            {
                target = range.ClosestTarget.GetComponent<IDamagable>();
            }

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
                MoveAlong();
                return;
            }
        }
    }
}
interface IDamagable
{
    void Damage(float d);
    float Health { get; }
    Transform transform { get; }
}