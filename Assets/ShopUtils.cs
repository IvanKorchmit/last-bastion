using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopUtils
{
    private static int money = 100;
    public static int Money => money;
    public static void GainMoney(int amount)
    {
        money += amount;
    }
}
