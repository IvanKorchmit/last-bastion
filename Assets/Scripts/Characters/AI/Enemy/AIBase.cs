using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using LastBastion.Waves;
using LastBastion.TimeSystem.Events;
using LastBastion.TimeSystem;
[RequireComponent(typeof(Stats))]
public class AIBase : MonoBehaviour, IUnsub
{
    [SerializeField] private bool winterResistant;
    public const float WINTER_SLOWDOWN = 1.5f;
    public static event System.Action OnEnemySpawn;
    public static event System.Action OnEnemyDeath;
    [SerializeField] private AudioClip acidDamage;
    private Vector2 moveDirection;
    private Stats stats;
    private Seeker seeker;
    private Path path;
    private RangeFinder range;
    [SerializeField] private float speed;
    private float initSpeed;
    [SerializeField] private bool isAttacking;
    protected IDamagable target;
    public const float BLOOD_MOON_MULT = 3f;
    private bool isBlizzard;
    private Vector2Int sectorIndex;
    private Vector2Int oldSectorIndex;
    protected virtual void Start()
    {
        initSpeed = speed;
        stats = GetComponent<Stats>();
        seeker = GetComponent<Seeker>();
        range = GetComponentInChildren<RangeFinder>();
        WeatherUtils.OnAcidRain += WeatherUtils_OnAcidRain;
        if (!winterResistant)
        {
            Calendar.OnWinter_Property += Calendar_OnWinter;
            Blizzard.OnBlizzard += Blizzard_OnBlizzard;
        }
        BloodMoon.OnBloodMoonChange += BloodMoon_OnBloodMoonChange;
        OnEnemySpawn?.Invoke();

    }

    private void Blizzard_OnBlizzard(bool obj)
    {
        isBlizzard = obj;
    }

    private void BloodMoon_OnBloodMoonChange(BloodMoon.BloodMoonStatus obj)
    {
        if (obj == BloodMoon.BloodMoonStatus.Begin)
        {
            speed = initSpeed * BLOOD_MOON_MULT;
            stats.IncreaseMeleeDamage(BLOOD_MOON_MULT);
        }
        else
        {
            speed = initSpeed;
            stats.IncreaseMeleeDamage(1);
        }
    }

    private void Calendar_OnWinter(bool isWinter)
    {
        speed = isWinter ? initSpeed / (WINTER_SLOWDOWN * (!isBlizzard ? 1f : 2f)) : initSpeed;
    }

    private void WeatherUtils_OnAcidRain(float value)
    {
        SoundManager.PlaySound(acidDamage, transform.position);
        stats.Damage(value, null);
    }
    private void FixedUpdate()
    {
        Sectors.AddGameObject(gameObject, out sectorIndex);
        if (sectorIndex != oldSectorIndex)
        {
            try
            {
                Sectors.RemoveGameObject(gameObject, oldSectorIndex);

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.StackTrace);
                Debug.Log(oldSectorIndex);
                throw;
            }
            oldSectorIndex = sectorIndex;
        }
    }
    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke();
        UnsubAll();
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

    public void UnsubAll()
    {
        WeatherUtils.OnAcidRain -= WeatherUtils_OnAcidRain;
        Calendar.OnWinter_Property -= Calendar_OnWinter;
        BloodMoon.OnBloodMoonChange -= BloodMoon_OnBloodMoonChange;
        Blizzard.OnBlizzard -= Blizzard_OnBlizzard;
    }
}
