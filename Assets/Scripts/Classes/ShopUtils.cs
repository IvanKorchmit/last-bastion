using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;
using System.Linq;
using UnityEngine.UI;
using Dialogue;
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
        if (money >= cost)
        {
            money -= cost;
            if (good != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 col = GameObject.Find(WavesUtils.COLONY_PATH).transform.position;
                var obj = MonoBehaviour.Instantiate(good.Prefab, good.MustCome ? col : pos, Quaternion.identity);
                if (obj.CompareTag("Player"))
                {
                    UnitAI uAI = obj.GetComponent<UnitAI>();
                    obj.GetComponent<Seeker>().StartPath(col, pos, uAI.OnPathCalculated);
                    uAI.Weapon = good.Weapon;
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
                            UnityEditor.EditorApplication.isPlaying = false;
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
namespace Dialogue
{
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
public static class DialogueUtils
{
    public static RectTransform dialoguePanelRef;
    public static GameObject choiceButton;
    public static void Dialogue(Dialogue.Content content)
    {
        dialoguePanelRef.gameObject.SetActive(true);
        TextMeshProUGUI text = dialoguePanelRef.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = content.Text;
        Transform buttons = dialoguePanelRef.Find("Buttons");
        while (buttons.childCount > 0)
        {
            Transform child = buttons.GetChild(0);
            child.SetParent(null);
            MonoBehaviour.Destroy(child.gameObject);
        }
        for (int i = 0; i < content.Choices.Length; i++)
        {
            Button b = MonoBehaviour.Instantiate(choiceButton, buttons).GetComponent<Button>();
            b.onClick.AddListener(content.Choices[i].OnChoice);
            b.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = content.Choices[i].Text;
        }
        
    }
    public static void CloseDIalogue()
    {
        dialoguePanelRef.gameObject.SetActive(false);
    }
}

public static class WavesUtils
{
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

    private static Animator lightAnimator;
    public static int WaveNumber => waveNumber;
    public static Animator LightAnimator => lightAnimator;
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
        public GameEvent gameEvent;
        public int number;
    }
    public static Animator CameraAnimatorReference => cameraAnimatorReference;
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
        for (int i = days.Length - 1; i >= 0; i--)
        {
            Day day = days[i];
            if (WavesUtils.WaveNumber >= day.number)
            {
                if (day.gameEvent != null)
                {
                    WeatherUtils.ApplyEvent(day.gameEvent);
                }
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
    private static GameEvent currentEvent = null;
    public enum Status
    {
        none, acid_rain
    }
    public static Status status;
    public static void Update()
    {
        switch (status)
        {
            case Status.none:
                break;
            case Status.acid_rain:
                {
                    IDamagable[] damagables = (IDamagable[])GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>().ToArray();

                    foreach (IDamagable d in damagables)
                    {
                        d.Damage(2, null);
                    }
                }
                break;
            default:
                break;
        }
    }
    public static void ApplyEvent(GameEvent e)
    {
        if (currentEvent != null)
        {
            currentEvent.End();
        }
        currentEvent = e;
        currentEvent.Launch();
    }
}


namespace TechnologyTree
{
    public static class TechTreeUtils
    {
        public static Branch techBranch;
    }
}