using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_BuyButton : MonoBehaviour
{
    public delegate void BuyDelegate(PurchaseInfo info);
    public static event BuyDelegate OnGoodSelection;
    [SerializeField] private Good good;
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

[System.Serializable]
public class Good
{
    [SerializeField] private bool isUnit;
    [SerializeField] private int cost;
    [SerializeField] private GameObject prefab;
    [SerializeField] private WeaponBase weapon;
    public int Cost => cost;
    public GameObject Prefab => prefab;
    public WeaponBase Weapon => weapon;
    public bool IsUnit => isUnit;
}
