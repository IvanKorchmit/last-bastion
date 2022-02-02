using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LastBastion.Economy;
public class Shop_BuyButton : MonoBehaviour
{
    public delegate void BuyDelegate(PurchaseInfo info);
    public static event BuyDelegate OnGoodSelection;
    [SerializeField] private Good good;
    [SerializeField] private float cooldown;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        Place.OnPlacecd += Place_OnPlacecd;
    }

    private void Place_OnPlacecd(PurchaseInfo info)
    {
        if (info.Good == good)
        {
            TimerUtils.AddTimer(cooldown, () => button.interactable = true);
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        if (ShopUtils.CanAfford(good.Cost))
        {
            if (good.IsUnit && HumanResourcesUtils.HumanResources == 0)
            {
                OnGoodSelection?.Invoke(new PurchaseInfo(PurchaseInfo._GoodType.Entity, PurchaseInfo.GoodOperation.Fail, null));
                return;
            }
            OnGoodSelection?.Invoke(new PurchaseInfo(good.IsUnit ? PurchaseInfo._GoodType.Unit
                : PurchaseInfo._GoodType.Entity, PurchaseInfo.GoodOperation.Success, good));


        }
        else
        {
            
            OnGoodSelection?.Invoke(new PurchaseInfo(PurchaseInfo._GoodType.Entity, PurchaseInfo.GoodOperation.Fail, null));
        }
    }
}

public class PurchaseInfo
{
    public enum GoodOperation
    {
        Success, Fail
    }
    public enum _GoodType
    {
        Unit, Entity
    }
    private GoodOperation goodOper;
    private _GoodType goodType;
    private Good good;
    public GoodOperation GoodStatus => goodOper;
    public _GoodType GoodType => goodType;
    public Good @Good => good;
    public PurchaseInfo(_GoodType type, GoodOperation status, Good good)
    {
        this.good = good;
        goodOper = status;
        goodType = type;
    }
}
