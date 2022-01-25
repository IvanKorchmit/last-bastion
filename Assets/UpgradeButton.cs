using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
[System.Serializable]
public struct UpgradeInfo
{
    public enum UpgradeType
    {
        Misc, Weapon, Unit, Unlock
    }
    public enum ParameterType
    {
        Speed, Damage, Misc
    }
    public UpgradeType type;
    public ParameterType param;
    public float value;
    public string target;
}
