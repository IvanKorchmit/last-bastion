using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopUtils
{
    public static UIShow UIPanel_Reference;
    private static int money = 100;
    public static int Money => money;
    public static void GainMoney(int amount)
    {
        money += amount;
    }
    public static void Buy(int cost, Good good)
    {
        if(money >= cost)
        {
            money -= cost;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var soldier = MonoBehaviour.Instantiate(good.Prefab, mousePos, Quaternion.identity);
            soldier.GetComponent<UnitAI>().Weapon = good.Weapon;

            
        }
    }
}

public static class Placement
{
    public static Good objectToPlace;
    public static bool canPlace;
}
[System.Serializable]
public class Good
{
    [SerializeField] private int cost;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Weapon weapon;
    public int Cost => cost;
    public GameObject Prefab => prefab;
    public Weapon @Weapon => weapon;
}
