using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIShow : MonoBehaviour
{
    public TextMeshProUGUI lowerPanelDisplayInfo;
    public TextMeshProUGUI mainMoneyText;
    public TextMeshProUGUI humanResourcesCounterText;
    public RectTransform shopWindow;
    public RectTransform dialoguePanel;
    public GameObject choiceButtonPrefab;
    public Image chaosBar;
    private void Start()
    {
        ShopUtils.UIPanel_Reference = this;
        ShopUtils.displayInfo_Reference = lowerPanelDisplayInfo;
        DialogueUtils.dialoguePanelRef = dialoguePanel;
        DialogueUtils.choiceButton = choiceButtonPrefab;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) ShowUpWindowShop();
    }
    private void OnGUI()
    {
        string s = WavesUtils.TimeRemaining > 0 ? "seconds" : "second";
        lowerPanelDisplayInfo.text = $"Time remaining: {WavesUtils.TimeRemaining} {s}; Day: {WavesUtils.WaveNumber};";
        mainMoneyText.text = $"{ShopUtils.Money}$";
        humanResourcesCounterText.text = $"H:{HumanResourcesUtils.HumanResources}";
        chaosBar.fillAmount = HumanResourcesUtils.Chaos / 1f;
    }
    public void ShowUpWindowShop()
    {
        shopWindow.gameObject.SetActive(!shopWindow.gameObject.activeSelf);
    }
    public void SkipCooldown()
    {
        while (WavesUtils.TimeRemaining > 0)
        {
            WavesUtils.DecrementTime();
        }
    }
}
