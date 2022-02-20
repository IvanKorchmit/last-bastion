using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LastBastion.Dialogue;
using LastBastion.Waves;
using System.Linq;
using UnityEngine.Events;
using LastBastion.Economy;
using LastBastion.TimeSystem;
public class UIShow : MonoBehaviour
{
    public UnityEvent onNight;
    public UnityEvent onDay;
    public static event System.Action OnDialogueClose;
    [SerializeField] private TextMeshProUGUI lowerPanelDisplayInfo;
    [SerializeField] private TextMeshProUGUI mainMoneyText;
    [SerializeField] private TextMeshProUGUI humanResourcesCounterText;
    [SerializeField] private RectTransform shopWindow;
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private RectTransform researchCanvas;
    [SerializeField] private GameObject sliderChoice;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private TextMeshProUGUI dayCounter;
    [SerializeField] private GameObject bossHealthPrefab;
    [SerializeField] private Transform bossHealthTransformPanel;
    [SerializeField] private Image chaosBar;
    [SerializeField] private GameObject weatherInfoPrefab;
    [SerializeField] private Transform weatherContents;
    [SerializeField] private TextMeshProUGUI resA;
    [SerializeField] private TextMeshProUGUI resB;
    [SerializeField] private TextMeshProUGUI resC;
    private void Start()
    {
        DialogueUtils.OnDialogueAppeared += DialogueUtils_OnDialogueAppeared;
        Shop_BuyButton.OnGoodSelection += Shop_BuyButton_OnGoodSelection;
        Place.OnPlacecd += Place_OnPlacecd;
        OnDialogueClose += UIShow_OnDialogueClose;
        SpawnerManager.OnBossSpawned += SpawnerManager_OnBossSpawned;
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
        if (WavesUtils.WaveNumber == 1)
        {
            SkipCooldown();
        }
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        if (obj == WavesUtils.DayTime.Day)
        {
            ShowWeather();
            while (bossHealthTransformPanel.childCount > 0)
            {
                GameObject temp = bossHealthTransformPanel.GetChild(0).gameObject;
                temp.transform.SetParent(null);
                Destroy(temp);
            }
            onDay?.Invoke();
        }
        else
        {
            onNight?.Invoke();
        }
    }
    private void ShowWeather()
    {
        while (weatherContents.childCount > 0)
        {
            GameObject temp = weatherContents.GetChild(0).gameObject;
            temp.transform.SetParent(null);
            Destroy(temp);
        }
        foreach (Calendar.Month month in Calendar.months)
        {
            Calendar.Day[] days = month.days;
            if (days.Last().number < WavesUtils.WaveNumber)
            {
                continue;
            }

            for (int i = WavesUtils.WaveNumber,j = 0; i + j < month.days.Length && j < 5; i++,j++)
            {
                Instantiate(weatherInfoPrefab, weatherContents).GetComponent<WeatherInfo>().Init(i + j);
            }
            break;
        }
    }
    private void SpawnerManager_OnBossSpawned(IDamagable[] obj)
    {
        while (bossHealthTransformPanel.childCount > 0)
        {
            GameObject temp = bossHealthTransformPanel.GetChild(0).gameObject;
            temp.transform.SetParent(null);
            Destroy(temp);
        }
       
        for (int i = 0; i < obj.Length; i++)
        {
            var bossHealth = Instantiate(bossHealthPrefab, bossHealthTransformPanel);
            bossHealth.GetComponentInChildren<ShowBossHealth>().Init(obj[i]);
        }
    }

    private void UIShow_OnDialogueClose()
    {
        dialoguePanel.gameObject.SetActive(false);
    }

    public static void CloseDialogue()
    {
#if UNITY_EDITOR
        try
        {
#endif
            OnDialogueClose?.Invoke();
#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
#endif
    }
    private void DialogueUtils_OnDialogueAppeared(DialogueContent content)
    {
#if UNITY_EDITOR
        try
        {
#endif
            dialoguePanel.gameObject.SetActive(true);
            TextMeshProUGUI text = dialoguePanel.Find("Content/TextRect/Text").GetComponent<TextMeshProUGUI>();
            text.text = content.Text;
            Transform buttons = dialoguePanel.Find("Buttons");
            while (buttons.childCount > 0)
            {
                Transform child = buttons.GetChild(0);
                child.SetParent(null);
                MonoBehaviour.Destroy(child.gameObject);
            }
            for (int i = 0; i < content.Choices.Length; i++)
            {
                if (content.Choices[i] is DialogueContent.ChoiceButton btn)
                {
                    Button b = MonoBehaviour.Instantiate(choiceButtonPrefab, buttons).GetComponent<Button>();
                    b.onClick.AddListener(btn.OnChoice);
                    b.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = btn.Text;
                }
                else if (content.Choices[i] is DialogueContent.ChoiceSlider sld)
                {
                    Slider s = Instantiate(sliderChoice, buttons).GetComponentInChildren<Slider>();
                    s.onValueChanged.AddListener(sld.OnSlider);
                    s.value = sld.Value;
                    s.minValue = sld.Min;
                    s.maxValue = sld.Max;
                    s.wholeNumbers = true;
                    s.GetComponentInParent<ChoiceSlider>().Init(sld);
                    
                }
            }

#if UNITY_EDITOR
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
#endif

    }

    private void Place_OnPlacecd(PurchaseInfo info)
    {
        TimerUtils.AddTimer(0.05f, () => shopWindow.gameObject.SetActive(true));
    }
    private void Shop_BuyButton_OnGoodSelection(PurchaseInfo info)
    {
        if (info.GoodStatus == PurchaseInfo.GoodOperation.Success)
        {
            shopWindow.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) ShowUpWindowShop();
    }
    private void OnGUI()
    {
        string s = WavesUtils.TimeRemaining > 0 ? "seconds" : "second";
        lowerPanelDisplayInfo.text = $"Time remaining: {WavesUtils.TimeRemaining} {s}";
        dayCounter.text = $"Day {WavesUtils.WaveNumber}";
        mainMoneyText.text = $"{ShopUtils.Money}$";
        humanResourcesCounterText.text = $"H:{HumanResourcesUtils.HumanResources}";
        resA.text = $"A:{ShopUtils.ResourceA}";
        resB.text = $"B:{ShopUtils.ResourceB}";
        resC.text = $"C:{ShopUtils.ResourceC}";
        chaosBar.fillAmount = HumanResourcesUtils.Chaos / 1f;
    }
    public void ShowUpWindowShop()  
    {
        shopWindow.gameObject.SetActive(!shopWindow.gameObject.activeSelf);
    }
    public void ShowUpResearchTree()
    {
        researchCanvas.gameObject.SetActive(!researchCanvas.gameObject.activeSelf);
    }
    public void SkipCooldown()
    {
        while (WavesUtils.TimeRemaining > 0)
        {
            WavesUtils.DecrementTime();
        }
    }
}
