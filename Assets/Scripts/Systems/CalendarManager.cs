using UnityEngine;
using LastBastion.TimeSystem.Events;
namespace LastBastion
{

    namespace TimeSystem
    {
        public class CalendarManager : MonoBehaviour
        {
            [SerializeField] GameEvent[] events;
            [SerializeField] private Calendar.Month[] months;
            private void Awake()
            {
                FillCalendar();
                Calendar.months = months;
            }
            private void FillCalendar()
            {
                int num = 0;
                for (int i = 0; i < months.Length; i++)
                {
                    for (int j = 0; j < months[i].days.Length; j++, num++)
                    {
                        ref Calendar.Day day = ref months[i].days[j];
                        day.number = num;
                        if (day.gameEvent == null)
                        {
                            day.gameEvent = GetRandomEvent();
                        }
                        if (day.weather != Calendar.Day.WeatherType.Winter)
                        {
                            day.weather = GetRandomWeatherType();
                        }

                    }
                }
            }
            private GameEvent GetRandomEvent()
            {
                return Random.value >= 0.75f ? events[Random.Range(0, events.Length)] : null;
            }

            private Calendar.Day.WeatherType GetRandomWeatherType()
            {
                System.Array vals = System.Enum.GetValues(typeof(Calendar.Day.WeatherType));
                Calendar.Day.WeatherType type = (Calendar.Day.WeatherType)vals.GetValue(Random.Range(0, vals.Length));
                if (type == Calendar.Day.WeatherType.Winter)
                {
                    type = GetRandomWeatherType();
                }
                return type;
            }
            private void Update()
            {
                TimerUtils.AddTimer(1, WeatherUtils.Update);
            }
        }
    }
}