using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LastBastion.Economy;
using TMPro;
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] bool invokeUnityEvent;
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
    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text += $"\n({upgradeCost}$ A:{resACost} B:{resBCost} C:{resCCost})";
    }
    public void OnClick()
    {
        if (ShopUtils.CanAfford(upgradeCost, resACost, resBCost, resCCost))
        {
            if (invokeUnityEvent)
            {
                OnUpgrade.Invoke();
            }
            ShopUtils.Buy(upgradeCost);
            ShopUtils.Buy(resACost, ShopUtils.ResourceType.A);
            ShopUtils.Buy(resBCost, ShopUtils.ResourceType.B);
            ShopUtils.Buy(resCCost, ShopUtils.ResourceType.C);
            OnUpgradeReceived?.Invoke(info);

        }
    }
}
