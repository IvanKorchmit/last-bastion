using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIShow : MonoBehaviour
{
    public TextMeshProUGUI lowerPanelDisplayInfo;
    public TextMeshProUGUI mainMoneyText;
    public TextMeshProUGUI armenText;
    public TextMeshProUGUI carboText;
    public TextMeshProUGUI plasmaText;
    public TextMeshProUGUI humanResourcesCounterText;
    public RectTransform shopWindow;
    private void Start()
    {
        ShopUtils.UIPanel_Reference = this;
        ShopUtils.displayInfo_Reference = lowerPanelDisplayInfo;
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
        armenText.text = $"A:{ShopUtils.Armenederdrnazite}";
        carboText.text = $"C:{ShopUtils.Carbomagnetite}";
        plasmaText.text = $"P:{ShopUtils.Plasma}";
        humanResourcesCounterText.text = $"H:{HumanResourcesUtils.HumanResources}";
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
