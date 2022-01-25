using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LastBastion.Dialogue;
using LastBastion.Waves;
public class UIShow : MonoBehaviour
{
    private static event System.Action OnDialogueClose;
    public TextMeshProUGUI lowerPanelDisplayInfo;
    public TextMeshProUGUI mainMoneyText;
    public TextMeshProUGUI humanResourcesCounterText;
    public RectTransform shopWindow;
    public RectTransform dialoguePanel;
    public RectTransform researchCanvas;
    public GameObject choiceButtonPrefab;
    public TextMeshProUGUI dayCounter;
    public GameObject bossHealthPrefab;
    public Transform bossHealthTransformPanel;
    public Image chaosBar;
    private void Start()
    {
        DialogueUtils.OnDialogueAppeared += DialogueUtils_OnDialogueAppeared;
        Shop_BuyButton.OnGoodSelection += Shop_BuyButton_OnGoodSelection;
        Place.OnPlacecd += Place_OnPlacecd;
        OnDialogueClose += UIShow_OnDialogueClose;
        SpawnerManager.OnBossSpawned += SpawnerManager_OnBossSpawned;
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        if (obj == WavesUtils.DayTime.Day)
        {
            researchCanvas.gameObject.SetActive(true);
            while (bossHealthTransformPanel.childCount > 0)
            {
                GameObject temp = bossHealthTransformPanel.GetChild(0).gameObject;
                temp.transform.SetParent(null);
                Destroy(temp);
            }
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
        OnDialogueClose?.Invoke();
    }
    private void DialogueUtils_OnDialogueAppeared(Content content)
    {
        dialoguePanel.gameObject.SetActive(true);
        TextMeshProUGUI text = dialoguePanel.Find("Content/Text").GetComponent<TextMeshProUGUI>();
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
            Button b = MonoBehaviour.Instantiate(choiceButtonPrefab, buttons).GetComponent<Button>();
            b.onClick.AddListener(content.Choices[i].OnChoice);
            b.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = content.Choices[i].Text;
        }
    }

    private void Place_OnPlacecd()
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
