using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using LastBastion.TimeSystem;
using LastBastion.TimeSystem.Events;
public class UnitAI : MonoBehaviour, ISelectable, IUnsub
{
    [SerializeField] private AudioClip acidDamage;
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
    private bool isFollowing;
    private static bool isBlizzard;
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
    public void Initialize(Vector2 target)
    {
        GetComponent<Seeker>().StartPath(transform.position, target, OnPathCalculated);
    }
    public void FindPath(Vector2 s, Vector2 e)
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
        AIBase.OnEnemyDeath += AIBase_OnEnemyDeath;
        Blizzard.OnBlizzard += Blizzard_OnBlizzard;
    }

    private void Blizzard_OnBlizzard(bool obj)
    {
        isBlizzard = obj;
    }

    private void AIBase_OnEnemyDeath()
    {
        if (weapon is Melee m)
            FollowEnemy(m);
        else if (isFollowing)
        {
            FollowEnemy(null);
        }
    }

    private void WeatherUtils_OnAcidRain(float value)
    {
        stats.Damage(value, null);
        SoundManager.PlaySound(acidDamage, transform.position);
    }

    private void Calendar_OnWinter(bool isWinter)
    {
        speed = isWinter ? initSpeed / (AIBase.WINTER_SLOWDOWN * (isBlizzard ? 2f : 1f)) : initSpeed;
    }

    protected virtual void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
        if (weapon is Firearm f)
        {
            if (isFollowing)
            {
                FollowEnemy(null);
            }
            if (range.ClosestTarget != null)
            {
                isAttacking = true;
                moveDirection = new Vector2();
                TimerUtils.AddTimer(f.Cooldown / f.SpeedDivision, Attack);
            }
            else
            {
                if (isAttacking && weapon is IWeaponStoppable stop)
                {
                    stop.Stop(shootPoint);
                }
                isAttacking = false;
                MoveAlong();
            }
        }
        else if (weapon is Melee m)
        {
            FollowEnemy(m);
        }
    }

    private void FollowEnemy(Melee m)
    {
        GameObject[] ens = GameObject.FindGameObjectsWithTag("Enemy");
        if (ens != null && ens.Length > 0)
        {
            GameObject en = ens[0];
            if (en != null && (path == null || path.path.Count == 0))
            {
                FindPath(transform.position, en.transform.position);
            }
            if (range.ClosestTarget != null && m != null)
            {
                TimerUtils.AddTimer(m.Cooldown, MeleeAttack);
            }
            else if (range.ClosestTarget == null)
            {
                if (isAttacking && weapon is IWeaponStoppable stop)
                {
                    stop.Stop(shootPoint);
                }
                isAttacking = false;
            }
        }
        MoveAlong();
    }

    public void Attack()
    {
        weapon.Use(shootPoint, range.ClosestTarget);
    }
    public void MeleeAttack()
    {
        if (weapon is Melee m)
        {
            if (range.ClosestTarget != null && isAttacking)
            {
                if (range.ClosestTarget.TryGetComponent(out IDamagable damage))
                {
                    damage.Damage(m.MeleeDamage, gameObject);
                    SoundManager.PlaySound(m.DamageSound, transform.position);
                }
            }
        }
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
            isFollowing = false;
            
        }
        else if (info.Command == InputInfo.CommandType.Follow)
        {
            isFollowing = true;
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
        AIBase.OnEnemyDeath -= AIBase_OnEnemyDeath;
        Blizzard.OnBlizzard -= Blizzard_OnBlizzard;
        PlayerInput.OnPlayerInput -= PlayerInput_OnPlayerInput;
    }
}
