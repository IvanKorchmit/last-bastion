using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
using System.Linq;
using UnityEngine.UI;
using LastBastion.Dialogue;
using LastBastion.Waves;
using LastBastion.TimeSystem.Events;
using UnityEngine.EventSystems;

public static class GameUtils
{
    public enum GameOverReason
    {
        Lose, DemoOver, CampaignComplete
    }
    public static void EndGame(GameOverReason reason)
    {
        DialogueContent.Choice[] Ok = new DialogueContent.Choice[] { new DialogueContent.Choice("End game",
                    () =>
                    {
                        if(!Application.isEditor)
                        {
                            Application.Quit();
                        }
                        else
                        {
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#endif
                        }
                        return true;
                        }, null, null)
                    };
        DialogueContent endContent = null;
        switch (reason)
        {
            case GameOverReason.Lose:
                {
                    endContent = new DialogueContent(Ok, "You have lost the game. Chaos has increased to high amounts and people are now leaving this order!");
                }
                break;
            case GameOverReason.DemoOver:

                endContent = new DialogueContent(Ok, "Thanks for playiing demo! Wait for further development later!!");

                break;
            case GameOverReason.CampaignComplete:
                break;
            default:
                break;
        }
        DialogueUtils.Dialogue(endContent);
    }
}
namespace LastBastion
{
    namespace Dialogue
    {
        public static class DialogueUtils
        {
            private static List<DialogueContent> pendingDialogues;

            public delegate void DialogueDelegate(DialogueContent content);
            public static event DialogueDelegate OnDialogueAppeared;
            public static void Dialogue(DialogueContent content)
            {
                pendingDialogues.Add(content);
                OnDialogueAppeared?.Invoke(content);
            }
            static DialogueUtils()
            {
                pendingDialogues = new List<DialogueContent>();
                UIShow.OnDialogueClose += UIShow_OnDialogueClose;
            }

            private static void UIShow_OnDialogueClose()
            {
                pendingDialogues.RemoveAt(0);
                Dialogue(pendingDialogues[0]);
            }

            public static DialogueContent.Choice[] OK => new DialogueContent.Choice[] { new DialogueContent.Choice("Okay",
            () => {
                UIShow.CloseDialogue();
                return true;
            }, null, null)
        };
            public static DialogueContent GenerateDialogue(string text, params DialogueContent.Choice[] choices)
            {
                return new DialogueContent(choices,text);
            }
            public static DialogueContent.Choice CreateChoice(string text, DialogueContent.Choice.ChoiceAction action, DialogueContent success, DialogueContent fail = null)
            {
                return new DialogueContent.Choice(text, action, success, fail);
            }
}
        public class DialogueContent
        {
            public class Choice
            {
                public delegate bool ChoiceAction();
                private ChoiceAction action;
                private string text;
                private DialogueContent alternativeResponse;
                private DialogueContent next;
                public Choice(string text, ChoiceAction action, DialogueContent next, DialogueContent alt)
                {
                    this.text = text;
                    this.action = action;
                    this.next = next;
                    alternativeResponse = alt;
                }
                public string Text => text;
                public void OnChoice()
                {
                    if (action())
                    {
                        if (next != null)
                        {
                            DialogueUtils.Dialogue(next);
                        }
                    }
                    else
                    {
                        DialogueUtils.Dialogue(alternativeResponse);
                    }
                }
            }
            private Choice[] choices;
            private string text;
            public string Text => text;
            public Choice[] Choices => choices;
            public DialogueContent(Choice[] choices, string text)
            {
                this.choices = choices;
                this.text = text;
            }
        }
    }
    namespace Economy
    {
        public struct FinanceProfile
        {
            public struct Debt
            {
                private BankBase bank; // Bank itself
                private int amount; // Amount of debt
                private int deadLine; // Days
                public int Amount => amount;
                public int Deadline => deadLine;
                public BankBase Bank => bank;
                public Debt(BankBase bank, int amount, int deadLine)
                {
                    this.bank = bank;
                    this.amount = amount;
                    this.deadLine = deadLine;
                }
                public void DeadlineDec()
                {
                    deadLine--;
                    if (deadLine <= 0)
                    {
                        bank.OnDeadlinePassed(amount);
                    }
                }
            }
            private int moneySpent;
            private int moneyReceived;
            private List<Debt> debts;
            public List<Debt> Debts => debts;
            public float Percentage
            {
                get
                {
                    if (moneySpent == 0)
                    {
                        return float.NaN;
                    }
                    return (moneyReceived - moneySpent) / moneySpent;
                }
            }
            public FinanceProfile(int moneySpent, int moneyReceived)
            {
                this.moneySpent = moneySpent;
                this.moneyReceived = moneyReceived;
                debts = new List<Debt>(5);
                WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
            }

            public void AddMoneyReceived(int value)
            {
                if (value <= 0) return;
                moneyReceived += value;
            }
            public void AddMoneySpent(int value)
            {
                if (value <= 0) return;
                moneySpent += value;
            }
            private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
            {
                if (obj == WavesUtils.DayTime.Day)
                {
                    foreach (Debt d in debts)
                    {
                        d.DeadlineDec();
                    }
                }
            }
        }
        public static class ShopUtils
        {
            public enum ResourceType
            {
                A, B, C
            }
            private static int money = 500;
            private static int resourceA = 0;
            private static int resourceB = 0;
            private static int resourceC = 0;
            private static int pendingMoney;
            private static FinanceProfile profile = new FinanceProfile(0,0);
            public static int ResourceA => resourceA;
            public static int ResourceB => resourceB;
            public static int ResourceC => resourceC;
            public static int Money => money;
            public static FinanceProfile Profile => profile;
            static ShopUtils()
            {
                WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
            }
            public static void AddDebtAndReceiveMoney(BankBase bank)
            {
                profile.Debts.Add(new FinanceProfile.Debt(bank, 1000, 5));
            }
            private static void WavesUtils_OnDayChanged(WavesUtils.DayTime dayTime)
            {
                if (dayTime == WavesUtils.DayTime.Day)
                {
                    money += pendingMoney;
                    profile.AddMoneyReceived(pendingMoney);
                    pendingMoney = 0;
                }
            }

            public static void GainMoney(int amount)
            {
                pendingMoney += amount;
            }
            public static void GainResource(ResourceType type, int amount)
            {
                switch (type)
                {
                    case ResourceType.A:
                        resourceA += amount;
                        break;
                    case ResourceType.B:
                        resourceB += amount;
                        break;
                    case ResourceType.C:
                        resourceC += amount;
                        break;
                    default:
                        break;
                }
            }
            public static bool CanAfford(int cost)
            {
                return money >= cost;
            }
            public static void Buy(Good good)
            {
                money -= good.Cost;
            }
            public static void Buy(int cost)
            {
                money -= cost;
            }
        }
    }
    namespace TimeSystem
    {
        public static class Calendar
        {
            [System.Serializable]
            public struct Month
            {
                public string name;
                public Day[] days;
            }
            private static Animator cameraAnimatorReference = Camera.main.GetComponent<Animator>();
            public static Month[] months;
            public delegate void WinterDelegate(bool isWinter);
            public delegate void FogDelegate(bool isFog);
            public static event FogDelegate OnFog;
            private static event WinterDelegate OnWinter;
            public static event WinterDelegate OnWinter_Property
            {
                add
                {
                    OnWinter += value;
                    Update();
                }
                remove
                {
                    OnWinter -= value;
                    Update();
                }
            }
            [System.Serializable]
            public struct Day
            {
                public enum WeatherType
                {
                    None, Winter, Fog
                }
                public WeatherType weather;
                public GameEvent gameEvent;
                public int number;
            }
            static Calendar()
            {
                WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
            }

            private static void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
            {
                if (obj == WavesUtils.DayTime.Day)
                {
                    CalculateResources();
                }
            }

            public static Animator CameraAnimatorReference => cameraAnimatorReference;
            public static void Update()
            {
                OnWinter?.Invoke(IsWinter(false));
            }
            public static void CalculateResources()
            {
                IsWinter(true);
                if (WavesUtils.WaveNumber % 3 == 0)
                {
                    HumanResourcesUtils.IncreaseHumanResources();
                }
            }
            public static bool IsWinter(bool doApply)
            {
                bool isWinter = false;
                foreach (Month month in months)
                {
                    Day[] days = month.days;
                    if (days.Last().number < WavesUtils.WaveNumber)
                    {
                        continue;
                    }
                    if (days.Last().number < WavesUtils.WaveNumber)
                    {
                        GameUtils.EndGame(GameUtils.GameOverReason.DemoOver);
                        return isWinter;
                    }
                    for (int i = days.Length - 1; i >= 0; i--)
                    {
                        Day day = days[i];
                        if (WavesUtils.WaveNumber >= day.number)
                        {
                            if (day.gameEvent != null)
                            {
                                if (doApply)
                                {
                                    WeatherUtils.ApplyEvent(day.gameEvent);
                                    if (day.weather == Day.WeatherType.Fog)
                                    {
                                        OnFog?.Invoke(true);
                                    }
                                    else
                                    {
                                        OnFog?.Invoke(false);
                                    }
                                }
                            }
                            else if (doApply)
                            {
                                WeatherUtils.ApplyEvent(null);
                                if (day.weather == Day.WeatherType.Fog)
                                {
                                    OnFog?.Invoke(true);
                                }
                                else
                                {
                                    OnFog?.Invoke(false);
                                }
                            }
                            return day.weather == Day.WeatherType.Winter;
                        }
                        else
                        {
                            isWinter = day.weather == Day.WeatherType.Winter;
                        }
                    }
                }
                return isWinter;
            }
        }

        public static class WeatherUtils
        {
            private static BoxCollider2D area = GameObject.Find("TechnicalStuff/MeteorArea").GetComponent<BoxCollider2D>();
            private static GameEvent currentEvent = null;
            public delegate void DamagableDel(float value);
            public static event DamagableDel OnAcidRain;
            public enum Status
            {
                None, AcidRain, MeteorRain
            }
            public static Status status;
            public static void Update()
            {
                if (WavesUtils.AreIncoming)
                {
                    switch (status)
                    {
                        case Status.None:
                            break;
                        case Status.AcidRain:
                            {
                                OnAcidRain?.Invoke(Random.Range(2f, 4f));
                            }
                            break;
                        case Status.MeteorRain:
                            if (Random.value >= 0.7f)
                            {
                                GameObject.Instantiate((currentEvent as MeteorRain).MeteorPrefab, SpawnerManager.PositionInside(area), Quaternion.identity);
                            }
                            break;
                    }
                }
            }
            public static void ApplyEvent(GameEvent e)
            {
                if (currentEvent != null && currentEvent is IEventEndable ev)
                {
                    ev.End();
                }
                currentEvent = e;
                currentEvent?.Launch();
            }
        }
    }
    namespace Waves
    {
        public static class WavesUtils
        {
            public enum DayTime
            {
                Day, Night
            }
            public static event System.Action<DayTime> OnDayChanged;
            private const int DeFAULT_TIME = 30;
            public const string TS_PATH = "TechnicalStuff";
            public const string COLONY_PATH = TS_PATH + "/Colony";
            private static int waveNumber = 1;
            private static int timeRemaining = DeFAULT_TIME;
            private static bool areIncoming = false;
            public static bool AreIncoming => areIncoming;

            public static void SetIncoming()
            {
                areIncoming = true;
            }

            public static int WaveNumber => waveNumber;
            public static void DecrementTime()
            {
                timeRemaining--;
            }
            public static int TimeRemaining => timeRemaining;
            public static void CheckRemainings()
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
                {
                    areIncoming = false;
                    timeRemaining = DeFAULT_TIME;
                    waveNumber++;
#if UNITY_EDITOR
                    try
                    {
#endif
                        OnDayChanged?.Invoke(DayTime.Day);
#if UNITY_EDITOR
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.StackTrace);
                    }
#endif
                }
            }
            public static WaveProps FindWave(WaveProps[] waves)
            {
                OnDayChanged?.Invoke(DayTime.Night);
                WaveProps temp = default(WaveProps);
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
            [SerializeField] private GameObject[] bosses;
            public int WaveNumber => waveNumber;
            public GameObject[] WaveEnemies => waveEnemies;
            public bool IsBoss => isBossWave;
            public GameObject[] Bosses => bosses;

        }
    }
}

public static class HumanResourcesUtils
{
    private static float chaos = 0f;
    private static int humanResources = 15;
    public static int HumanResources => humanResources;
    public static float Chaos => chaos;

    public static void IncreaseChaos(float v)
    {
        if (chaos + v >= 1)
        {
            GameUtils.EndGame(GameUtils.GameOverReason.Lose);
            chaos += v;
            chaos = Mathf.Clamp01(chaos);
            return;
        }
        chaos += v;
    }
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
        humanResources += UnityEngine.Random.Range(2, 5);
    }
}

public class InputInfo
{
    public enum CommandType
    {
        Move, Deselect, Follow
    }
    private CommandType command;
    private Vector2 position;
    public CommandType Command => command;
    public Vector2 Position => position;
    public InputInfo(Vector2 position, CommandType command)
    {
        this.position = position;
        this.command = command;
    }
}

public static class Sectors
{
    public const int SECTOR_SIZE = 2;
    private static Vector2Int size = new Vector2Int(40, 20);
    public static Vector2Int GridSize => size;
    public static Vector2 OriginPosition => origin.position;
    private class Sector
    {
        public List<GameObject> contents = new List<GameObject>(5);
    }
    private static Transform origin = GameObject.Find("SectorOrigin").transform;
    private static Sector[,] grid = new Sector[size.x, size.y];
    static Sectors()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new Sector();
            }
        }
        AIBase.OnEnemyDeath += AIBase_OnEnemyDeath;
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
        ColonySystem.OnEnemyEnter += ColonySystem_OnEnemyEnter;
    }

    private static void ColonySystem_OnEnemyEnter()
    {
        ClearNulls();
    }
    private static void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        ClearNulls();
    }

    private static void ClearNulls()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y].contents.RemoveAll((m) => m == null);
            }
        }
    }
    private static void AIBase_OnEnemyDeath()
    {
        ClearNulls();
    }

    public static void AddGameObject(GameObject obj, out Vector2Int index)
    {
        if (obj == null)
        {
            index = Vector2Int.zero;
            return;
        }
        Sector s = SectorAtPosition(obj.transform.position, out Vector2Int result);
        if (s.contents.Contains(obj))
        {
            index = Vector2Int.RoundToInt(result);
            return;
        }
        s.contents.Add(obj);
        index = result;
    }
    public static void RemoveGameObject(GameObject obj, Vector2Int index)
    {
        if (obj == null) return;
        Sector s = grid[index.x, index.y];
        if (s.contents.Contains(obj))
        {
            s.contents.Remove(obj);
        }
    }
    public static bool HasSomething(Vector2Int ind)
    {
        return grid[ind.x, ind.y].contents.Count > 0;
    }
    private static Sector SectorAtPosition(Vector2 position, out Vector2Int result)
    {
        Vector2Int index = PositionToSectorIndex(position);
        result = index;
        return grid[index.x, index.y];
    }
    public static Vector2Int PositionToSectorIndex(Vector2 position)
    {
        position = Vector2Utils.Ceil(position);
        Vector2 proc = origin.InverseTransformPoint(position);
        Vector2Int index = Vector2Int.RoundToInt(proc / SECTOR_SIZE);
        return index;
    }
    public static Vector2 PositionAtSector(Vector2Int index)
    {
        Vector2 result = origin.TransformPoint((Vector2)index);
        return result;
    }


}

public static class Vector2Utils
{
    public static Vector2 Abs(this Vector2 vec)
    {
        return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }
    public static Vector2 Round(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }
    public static Vector2 Ceil(Vector2 vec)
    {
        return new Vector2(Mathf.Ceil(vec.x), Mathf.Ceil(vec.y));
    }
    public static Vector2 Floor(Vector2 vec)
    {
        return new Vector2(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
    }
}

public interface ISelectable
{
    void OnSelect();
    void OnDeselect();
}
public interface IEventEndable
{
    void End();
}

public interface IUnsub
{
    void UnsubAll();
}
