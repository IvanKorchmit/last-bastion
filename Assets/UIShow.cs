using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIShow : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public RectTransform shopWindow;
    private void Start()
    {
        ShopUtils.UIPanel_Reference = this;
    }
    private void OnGUI()
    {
        moneyText.text = $"Balance: {ShopUtils.Money}";
    }
    public void ShowUpWindowShop()
    {
        shopWindow.gameObject.SetActive(!shopWindow.gameObject.activeSelf);
    }
}
