using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
public static class ShopUtils
{
    public static UIShow UIPanel_Reference;
    public static TextMeshProUGUI displayInfo_Reference;
    private static int money = 500;
    public static int Money => money;
    public static void GainMoney(int amount)
    {
        money += amount;
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

    public const string TS_PATH = "TechnicalStuff";
    public const string COLONY_PATH = TS_PATH + "/Colony";
    public static int waveNumber = 1;
    public static int timeRemaining = 10;
    public static bool areIncoming = false;
    private static Animator lightAnimator;
    static WavesUtils()
    {
        lightAnimator = GameObject.Find($"{TS_PATH}/Light").GetComponent<Animator>();
    }
    public static void CheckRemainings()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            lightAnimator.SetBool("isDay", true);
            areIncoming = false;
            timeRemaining = 10;
            waveNumber++;
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
