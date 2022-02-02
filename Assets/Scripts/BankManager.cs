using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.Economy;
public class BankManager : MonoBehaviour
{
    [SerializeField] private BankBase[] banks;
    private void Start()
    {
        foreach (var item in banks)
        {
            item.Awake();
        }
    }
}
