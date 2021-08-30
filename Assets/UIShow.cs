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
    public RectTransform shopWindow;
    private void Start()
    {
        ShopUtils.UIPanel_Reference = this;
        ShopUtils.displayInfo_Reference = lowerPanelDisplayInfo;
    }
    private void OnGUI()
    {
        string s = WavesUtils.timeRemaining > 0 ? "seconds" : "second";
        lowerPanelDisplayInfo.text = $"Time remaining: {WavesUtils.timeRemaining} {s}";
        mainMoneyText.text = $"{ShopUtils.Money}$";
        armenText.text = $"A:{ShopUtils.Armenederdrnazite}";
        carboText.text = $"C:{ShopUtils.Carbomagnetite}";
        plasmaText.text = $"P:{ShopUtils.Plasma}";
    }
    public void ShowUpWindowShop()
    {
        shopWindow.gameObject.SetActive(!shopWindow.gameObject.activeSelf);
    }
}
