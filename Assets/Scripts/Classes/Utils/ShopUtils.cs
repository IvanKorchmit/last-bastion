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
        DialogueContent.ChoiceButton[] Ok = new DialogueContent.ChoiceButton[] { new DialogueContent.ChoiceButton("End game",
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
                if (pendingDialogues.Count > 0)
                {
                    Dialogue(pendingDialogues[0]);
                }
            }
            public static DialogueContent.ChoiceButton[] OK => new DialogueContent.ChoiceButton[] { new DialogueContent.ChoiceButton("Okay", () => { UIShow.CloseDialogue(); return true; }, null, null)};
        
            public static DialogueContent GenerateDialogue(string text, params DialogueContent.Choice[] choices)
            {
                return new DialogueContent(choices, text);
            }
            public static DialogueContent.ChoiceButton CreateChoiceButton(string text, DialogueContent.Choice.ChoiceAction action, DialogueContent success, DialogueContent fail = null)
            {
                return new DialogueContent.ChoiceButton(text, action, success, fail);
            }
            public static DialogueContent.ChoiceSlider CreateSlider(string format, int min, int max)
            {
                return new DialogueContent.ChoiceSlider(format, min, max);
            }
        }
        public class DialogueContent
        {
            public abstract class Choice
            { 
                public delegate bool ChoiceAction();

            }
            public class ChoiceButton : DialogueContent.Choice
            {
                private DialogueContent alternativeResponse;
                private DialogueContent next;
                private ChoiceAction action;
                private string text;
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
                public ChoiceButton(string text, ChoiceAction action, DialogueContent next, DialogueContent alt)
                {
                    this.action = action;
                    this.next = next;
                    this.text = text;
                    alternativeResponse = alt;
                }
            }
            public class ChoiceSlider : DialogueContent.Choice
            {
                private string format;
                private string displayText;
                private int min;
                private int max;
                private int value;
                public static event System.Action<ChoiceSlider> OnValueChanged;
                public string Text => displayText;
                public int Min => min;
                public int Max => max;
                public int Value => value;
                public void OnSlider(float value)
                {
                    this.value = (int)value;
                    displayText = string.Format(format, this.value);
                    OnValueChanged?.Invoke(this);
                }
                public ChoiceSlider(string formattedText, int min, int max)
                {
                    this.format = formattedText;
                    this.min = min;
                    this.max = max;
                    value = min;
                }
            }
            private Choice[] choices;
            private bool success;
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
            private int _moneySpent;
            private int _moneyReceived;
            private List<Debt> debts;
            public List<Debt> Debts => debts;
            public float Percentage
            {
                get
                {
                    if (_moneySpent == 0)
                    {
                        return float.NaN;
                    }
                    return (float)(_moneyReceived - _moneySpent) / (float)_moneySpent;
                }
            }
            public FinanceProfile(int moneySpent, int moneyReceived)
            {
                this._moneySpent = moneySpent;
                this._moneyReceived = moneyReceived;
                debts = new List<Debt>(5);
                WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
            }

            public void AddMoneyReceived(int value)
            {
                _moneyReceived += value;
            }
            public void AddMoneySpent(int value)
            {
                _moneySpent += value;
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
            public override string ToString()
            {
                string res = $"Spent: {_moneySpent}\nReceived:{_moneyReceived}\nPercentage:{Percentage}";
                return res;
            }
        }
        public static class ShopUtils
        {
            public enum ResourceType
            {
                A, B, C
            }
            private static int _money = 500;
            private static int _resourceA = 0;
            private static int _resourceB = 0;
            private static int _resourceC = 0;
            private static int _pendingMoney;
            private static FinanceProfile _profile = new FinanceProfile(0, 0);
            public static int ResourceA => _resourceA;
            public static int ResourceB => _resourceB;
            public static int ResourceC => _resourceC;
            public static int Money => _money;
            public static FinanceProfile Profile => _profile;
            static ShopUtils()
            {
                WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
            }
            public static void AddDebtAndReceiveMoney(BankBase bank, int amount)
            {
                _profile.Debts.Add(new FinanceProfile.Debt(bank, amount, bank.Deadline));
                _money += amount;
            }
            public static void RemoveDebts(BankBase bank)
            {
                _profile.Debts.RemoveAll((currentDebt) => currentDebt.Bank == bank);
            }
            private static void WavesUtils_OnDayChanged(WavesUtils.DayTime dayTime)
            {
                if (dayTime == WavesUtils.DayTime.Day)
                {
                    _money += _pendingMoney;
                    _profile.AddMoneyReceived(_pendingMoney);
                    _pendingMoney = 0;
                }
            }

            public static void GainMoney(int amount)
            {
                _pendingMoney += amount;
            }
            public static void GainResource(ResourceType type, int amount)
            {
                switch (type)
                {
                    case ResourceType.A:
                        _resourceA += amount;
                        break;
                    case ResourceType.B:
                        _resourceB += amount;
                        break;
                    case ResourceType.C:
                        _resourceC += amount;
                        break;
                    default:
                        break;
                }
            }
            public static bool CanAfford(int cost)
            {
                return _money >= cost;
            }
            public static bool CanAfford(int cost, int a, int b, int c)
            {
                return _money >= cost && _resourceA >= a && _resourceB >= b && _resourceC >= c;
            }
            public static void Buy(Good good)
            {
                _money -= good.Cost;
                _profile.AddMoneySpent(good.Cost);
            }
            public static void Buy(int cost)
            {
                _money -= cost;
                _profile.AddMoneySpent(cost);
            }
            public static void Buy(int cost, ResourceType type)
            {
                switch (type)
                {
                    case ResourceType.A:
                        _resourceA -= cost;
                        break;
                    case ResourceType.B:
                        _resourceB -= cost;
                        break;
                    case ResourceType.C:
                        _resourceC -= cost;
                        break;
                }
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
            public static void ChangeEvent(int day, GameEvent e)
            {
                ref Month cur = ref CurrentMonth;
                if (cur.days.Last().number <= day)
                {
                    cur = ref months[CurrentMonthIndex + 1];
                    day = cur.days.Length - day;
                }
                Debug.Log("Day " + day);
                cur.days[day].gameEvent = e;
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
            public static int CurrentMonthIndex
            {
                get
                {
                    Month m = months[0];
                    for (int i = 0; i < months.Length; i++)
                    {
                        m = months[i];
                        Day[] days = m.days;
                        if (days.Last().number > WavesUtils.WaveNumber)
                        {
                            return i;
                        }
                    }
                    return 0;
                }
            }
            public static ref Month CurrentMonth
            {
                get
                {
                    ref Month m = ref months[0];
                    for (int i = 0; i < months.Length; i++)
                    {
                        m = ref months[i];
                        Day[] days = m.days;
                        if (days.Last().number > WavesUtils.WaveNumber)
                        {
                            return ref m;
                        }
                    }
                    return ref m;
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
            private static int _waveNumber = 1;
            private static int _timeRemaining = DeFAULT_TIME;
            private static bool _areIncoming = false;
            public static bool AreIncoming => _areIncoming;

            public static void SetIncoming()
            {
                _areIncoming = true;
            }
            static WavesUtils()
            {
                AIBase.OnEnemyDeath += CheckRemainings;
            }

            public static int WaveNumber => _waveNumber;
            public static void DecrementTime()
            {
                _timeRemaining--;
            }
            public static int TimeRemaining => _timeRemaining;
            public static void CheckRemainings()
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
                {
                    _areIncoming = false;
                    _timeRemaining = DeFAULT_TIME;
                    _waveNumber++;
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
                    if (w.WaveNumber <= _waveNumber)
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
        humanResources += UnityEngine.Random.Range(2, 5);
    }
    public static void IncreaseHumanResources(int amount)
    {
        humanResources += amount;
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
    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
    {
        value.x = Mathf.Clamp(value.x, min.x, max.x);
        value.y = Mathf.Clamp(value.y, min.y, max.y);
        return value;
    }
    public static Vector2Int IntMod(Vector2Int a, Vector2Int b)
    {
        Vector2Int result = a;
        a.x %= b.x;
        a.y %= b.y;
        return result;
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
