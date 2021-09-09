using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
public static class ShopUtils
{
    public enum MineralType
    {
        Armenederdrnazite, Plasma, Pheromenirite, Carbomagnetite
    }
    public static UIShow UIPanel_Reference;
    public static TextMeshProUGUI displayInfo_Reference;
    private static int money = 500;
    private static int armenederdrnazite;
    private static int plasma;
    private static int pherominirite;
    private static int carbomagnetite;
    /// <summary>
    /// Basic mineral for creating weapons
    /// </summary>
    public static int Armenederdrnazite => armenederdrnazite;
    /// <summary>
    /// Energy substance
    /// </summary>
    public static int Plasma => plasma;
    /// <summary>
    /// Unknown
    /// </summary>
    public static int Pherominirite => pherominirite;
    /// <summary>
    /// Unknown
    /// </summary>
    public static int Carbomagnetite => carbomagnetite;
    public static int Money => money;
    public static void GainMoney(int amount)
    {
        money += amount;
    }
    public static void GainResource(int amount, MineralType type)
    {
        switch (type)
        {
            case MineralType.Armenederdrnazite:
                armenederdrnazite += amount;
                break;
            case MineralType.Plasma:
                plasma += amount;
                break;
            case MineralType.Pheromenirite:
                pherominirite += amount;
                break;
            case MineralType.Carbomagnetite:
                carbomagnetite += amount;
                break;
        }
    }
    public static void Buy(int cost, Good good)
    {
        if(money >= cost)
        {
            money -= cost;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 col = GameObject.Find(WavesUtils.COLONY_PATH).transform.position;
            var obj = MonoBehaviour.Instantiate(good.Prefab, good.MustCome ? col : pos, Quaternion.identity);
            if(obj.CompareTag("Player"))
            {
                UnitAI uAI = obj.GetComponent<UnitAI>();
                obj.GetComponent<Seeker>().StartPath(col, pos, uAI.OnPathCalculated);
                uAI.Weapon = good.Weapon;
                if (uAI is MinerAI m)
                {
                    RaycastHit2D r = Physics2D.CircleCast(pos, 1f, Vector2.zero, 0);
                    if (r.collider != null && r.collider.TryGetComponent(out Ore ore))
                    {
                        m.OreTarget = ore;
                    }
                }
            }
        }
    }
}

public static class Placement
{
    public static Good objectToPlace;
    public static bool canPlace;
}
[System.Serializable]
public class Good
{
    [SerializeField] private int cost;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Weapon weapon;
    [SerializeField] private bool mustCome;
    public int Cost => cost;
    public GameObject Prefab => prefab;
    public Weapon @Weapon => weapon;
    public bool MustCome => mustCome;
}
public static class WavesUtils
{
    private const int DeFAULT_TIME = 30;
    public const string TS_PATH = "TechnicalStuff";
    public const string COLONY_PATH = TS_PATH + "/Colony";
    private static int waveNumber = 1;
    private static int timeRemaining = DeFAULT_TIME;
    public static bool areIncoming = false;
    private static Animator lightAnimator;
    public static int WaveNumber => waveNumber;
    static WavesUtils()
    {
        lightAnimator = GameObject.Find($"{TS_PATH}/Light").GetComponent<Animator>();
    }
    public static void DecrementTime()
    {
        timeRemaining--;
    }
    public static int TimeRemaining => timeRemaining;
    public static void CheckRemainings()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            lightAnimator.SetBool("isDay", true);
            areIncoming = false;
            timeRemaining = DeFAULT_TIME;
            waveNumber++;
            Calendar.Update();
        }
    }
    public static WaveProps FindWave(WaveProps[] waves)
    {
        WaveProps temp = default(WaveProps);
        lightAnimator.SetBool("isDay", false);
        foreach (WaveProps w in waves)
        {
            if (w.WaveNumber <= waveNumber)
            {
                temp = w;
            }
        }
        return temp;
    }
    
}

[System.Serializable]
public class WaveProps
{
    [SerializeField] private int waveNumber;
    [SerializeField] private GameObject[] waveEnemies;
    [SerializeField] private bool isBossWave;
    public int WaveNumber => waveNumber;
    public GameObject[] WaveEnemies => waveEnemies;
}
public static class Calendar
{
    private static Animator cameraAnimatorReference = Camera.main.GetComponent<Animator>();
    public static Day[] days;
    [System.Serializable]
    public struct Day
    {
        public bool isWinter;
        public int number;
    }
    public static void Update()
    {
        cameraAnimatorReference.SetBool("isWinter", IsWinter());
        if (WavesUtils.WaveNumber % 3 == 0)
        {
            HumanResourcesUtils.IncreaseHumanResources();
        }
    }
    public static bool IsWinter()
    {
        bool isWinter = false;
        foreach (Day day in days)
        {
            if (WavesUtils.WaveNumber >= day.number)
            {
                return day.isWinter;
            }
            else
            {
                isWinter = day.isWinter;
            }
        }
        return isWinter;
    }
}

public static class HumanResourcesUtils
{
    private static float chaos = 0f;
    private static int humanResources = 5;

    public static int HumanResources => humanResources;

    public static bool TakeOne()
    {
        if (humanResources > 0)
        {
            humanResources--;
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void IncreaseHumanResources()
    {
        //humanResources = Mathf.RoundToInt(humanResources + (float)humanResources * 1.1f);
        humanResources += Random.Range(2, 5);
    }
}
