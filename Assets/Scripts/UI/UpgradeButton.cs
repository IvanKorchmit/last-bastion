using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LastBastion.Economy;
using LastBastion.Waves;
using UnityEngine.UI;
using TMPro;
public class UpgradeButton : MonoBehaviour
{
    public delegate void UpgradeDelegate(UpgradeInfo upgradeInfo);
    public static event UpgradeDelegate OnUpgradeReceived;
    [SerializeField]
    private UpgradeInfo info;
    [SerializeField]
    private UnityEvent OnUpgrade;
    [SerializeField]
    private int upgradeCost;
    [SerializeField] private int resACost;
    [SerializeField] private int resBCost;
    [SerializeField] private int resCCost;
    [SerializeField] private int daysRemaining = 2;
    private bool hasStarted;
    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text += $"\n({upgradeCost}$ A:{resACost} B:{resBCost} C:{resCCost})";
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime time)
    {
        if (time == WavesUtils.DayTime.Day && hasStarted)
        {
            daysRemaining--;
            if (daysRemaining <= 0)
            {
                OnUpgrade.Invoke();
                OnUpgradeReceived?.Invoke(info);

            }
        }
    }

    public void OnClick()
    {
        if (ShopUtils.CanAfford(upgradeCost, resACost, resBCost, resCCost))
        {
            ShopUtils.Buy(upgradeCost);
            ShopUtils.Buy(resACost, ShopUtils.ResourceType.A);
            ShopUtils.Buy(resBCost, ShopUtils.ResourceType.B);
            ShopUtils.Buy(resCCost, ShopUtils.ResourceType.C);
            GetComponent<Button>().interactable = false;
            hasStarted = true;
        }
    }
}
