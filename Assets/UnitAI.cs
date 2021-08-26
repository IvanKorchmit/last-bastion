using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitAI : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private RangeFinder range;
    private bool isAttacking;
    private Path path;
    [SerializeField] private float speed;
    private Seeker seeker;
    private Vector2 moveDirection;
    public Weapon @Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }

    }
    public void OnPathCalculated(Path p)
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
            if (range.ClosestTarget != null)
            {
                seeker.StartPath(transform.position,
                    range.ClosestTarget.position, OnPathCalculated);
            }
        }
        if (path != null)
        {
            if (range.ClosestTarget != null)
            {
                seeker.StartPath(transform.position,
                    range.ClosestTarget.position, OnPathCalculated);
                isAttacking = true;
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
                    moveDirection = Vector2.zero;
                    path = null;
                }
            }
        }
    }
    private void Start()
    {
        range = GetComponentInChildren<RangeFinder>();
    }
    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
        if (weapon is Firearm f)
        {
            if (range.ClosestTarget != null)
            {
                isAttacking = true;
                TimerUtils.AddTimer(f.Cooldown, Attack);
            }
            else
            {
                isAttacking = false;
            }
        }
        MoveAlong();
    }
    public void Attack()
    {
        weapon.Use(gameObject, range.ClosestTarget);
    }
}
