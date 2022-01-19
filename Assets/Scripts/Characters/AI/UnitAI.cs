using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
public class UnitAI : MonoBehaviour, ISelectable, IUnsub
{
    private Stats stats;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private WeaponBase weapon;
    protected RangeFinder range;
    private bool isAttacking;
    protected Path path;
    [SerializeField] private float speed;
    private float initSpeed;
    private Seeker seeker;
    protected Vector2 moveDirection;
    [SerializeField] private Image hpBar;
    public WeaponBase @Weapon
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
        if (!p.error)
        {
            path = p;
        }
    }
    protected void FindPath(Vector2 s, Vector2 e)
    {
        seeker.StartPath(s, e, OnPathCalculated);
    }
    protected void MoveAlong()
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
                seeker.StartPath
                    (
                    transform.position,
                    range.ClosestTarget.position,
                    OnPathCalculated
                    );
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
    protected virtual void Start()
    {
        range = GetComponentInChildren<RangeFinder>();
        stats = GetComponent<Stats>();
        seeker = GetComponent<Seeker>();
        initSpeed = speed;
        if (weapon is Melee m)
        {
            TimerUtils.AddTimer(0.02f, () => range.Radius = m.Range);
        }
        Calendar.OnWinter_Property += Calendar_OnWinter;
        WeatherUtils.OnAcidRain += WeatherUtils_OnAcidRain;
    }

    private void WeatherUtils_OnAcidRain(float value)
    {
        stats.Damage(value, null);
    }

    private void Calendar_OnWinter(bool isWinter)
    {
        speed = isWinter ? initSpeed / 3 : initSpeed;
    }

    protected virtual void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
        if (weapon is Firearm f)
        {
            if (range.ClosestTarget != null)
            {
                isAttacking = true;
                moveDirection = new Vector2();
                TimerUtils.AddTimer(f.Cooldown, Attack);
            }
            else
            {
                isAttacking = false;
                MoveAlong();
            }
        }
        else if (weapon is Melee m)
        {
            GameObject[] ens = GameObject.FindGameObjectsWithTag("Enemy");
            if (ens != null && ens.Length > 0)
            {
                GameObject en = ens[0];
                if (en != null && (path == null || path.path.Count == 0))
                {
                    FindPath(transform.position, en.transform.position);
                }
                if (range.ClosestTarget != null)
                {
                    TimerUtils.AddTimer(m.Cooldown, () =>
                     {
                         if (range.ClosestTarget != null)
                         {
                             if (range.ClosestTarget.TryGetComponent(out IDamagable damage))
                             {
                                 damage.Damage(m.MeleeDamage, gameObject);
                             }
                         }
                     });
                }
                else
                {
                    isAttacking = false;
                }
            }
            MoveAlong();
        }
    }
    public void Attack()
    {
        weapon.Use(shootPoint, range.ClosestTarget);
    }

    public void OnSelect()
    {
        PlayerInput.OnPlayerInput += PlayerInput_OnPlayerInput;
        hpBar.color = Color.yellow;
    }

    public void OnDeselect()
    {
        PlayerInput.OnPlayerInput -= PlayerInput_OnPlayerInput;
        hpBar.color = Color.green;

    }

    private void PlayerInput_OnPlayerInput(InputInfo info)
    {
        if (info.Command == InputInfo.CommandType.Move)
        {
            FindPath(transform.position, info.Position);
            
        }
        else if (info.Command == InputInfo.CommandType.Deselect)
        {
            OnDeselect();
        }
    }
    public void UnsubAll()
    {
        Calendar.OnWinter_Property -= Calendar_OnWinter;
        WeatherUtils.OnAcidRain -= WeatherUtils_OnAcidRain;
    }
}


public interface IUnsub
{
    void UnsubAll();
}
