﻿using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    [SerializeField]
    private Calendar.Day[] days;
    private void Start()
    {
        Calendar.days = days;
    }
    private void Update()
    {
        TimerUtils.AddTimer(1, WeatherUtils.Update);
    }
}