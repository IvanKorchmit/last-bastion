using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.Economy;
using LastBastion.Waves;
public class BankManager : MonoBehaviour
{
    [SerializeField] private BankBase[] banks;
    private void Start()
    {
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        if (obj == WavesUtils.DayTime.Day)
        {
            banks[0].Awake();
        }
    }
}
