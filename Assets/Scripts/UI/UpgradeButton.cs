using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LastBastion.Economy;
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

    public void OnClick()
    {
        if (ShopUtils.CanAfford(upgradeCost))
        {
            if (invokeUnityEvent)
            {
                OnUpgrade.Invoke();
            }
               ShopUtils.Buy(upgradeCost);
            OnUpgradeReceived?.Invoke(info);

        }
    }
}
