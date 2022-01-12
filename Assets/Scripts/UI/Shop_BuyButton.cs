using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_BuyButton : MonoBehaviour
{
    [SerializeField] private Good good;
    public void OnClick()
    {
        Placement.objectToPlace = good;
        ShopUtils.UIPanel_Reference.shopWindow.gameObject.SetActive(false);
    }
}
