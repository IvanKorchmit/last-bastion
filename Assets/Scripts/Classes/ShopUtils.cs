using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
using System.Linq;
using UnityEngine.UI;
using LastBastion.Dialogue;
using LastBastion.Waves;
using UnityEngine.EventSystems;

public static class ShopUtils
{
    public enum ResourceType
    {
        A,B,C
    }
    private static int money = 10000;
    private static int resourceA = 0;
    private static int resourceB = 0;
    private static int resourceC = 0;
    public static int ResourceA => resourceA;
    public static int ResourceB => resourceB;
    public static int ResourceC => resourceC;
    public static int Money => money;
    public static void GainMoney(int amount)
    {
        money += amount;
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
public static class GameUtils
{
    public enum GameOverReason
    {
        Lose, DemoOver, CampaignComplete
    }
    public static void EndGame(GameOverReason reason)
    {
        Content.Choice[] Ok = new Content.Choice[] { new Content.Choice("End game",
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
        Content endContent = null;
        switch (reason)
        {
            case GameOverReason.Lose:
                {
                    endContent = new Content(Ok, "You have lost the game. Chaos has increased to high amounts and people are now leaving this order!");
                }
                break;
            case GameOverReason.DemoOver:

                endContent = new Content(Ok, "Thanks for playiing demo! Wait for further development later!!");

                break;
            case GameOverReason.CampaignComplete:
                break;
            default:
                break;
        }
        DialogueUtils.Dialogue(endContent);
    }
}
namespace LastBastion.Dialogue
{
    public static class DialogueUtils
    {
        public delegate void DialogueDelegate(Content content);
        public static event DialogueDelegate OnDialogueAppeared;
        public static void Dialogue(Content content)
        {
            OnDialogueAppeared?.Invoke(content);

        }
    }
    public class Content
    {
        public class Choice
        {
            public delegate bool ChoiceAction();
            private ChoiceAction action;
            private string text;
            private Content alternativeResponse;
            private Content next;
            public Choice(string text, ChoiceAction action, Content next, Content alt)
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
        public Content(Choice[] choices, string text)
        {
            this.choices = choices;
            this.text = text;
        }
    }
}
namespace LastBastion.Waves
{
    public static class WavesUtils
    {
        static WavesUtils()
        {
        }
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


public interface ISelectable
{
    void OnSelect();
    void OnDeselect();
}