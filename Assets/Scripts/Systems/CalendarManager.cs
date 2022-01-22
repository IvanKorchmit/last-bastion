using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    [SerializeField]
    private Calendar.Month[] months;
    private void Awake()
    {
        Calendar.months = months;
    }
    private void Update()
    {
        TimerUtils.AddTimer(1, WeatherUtils.Update);
    }
}
