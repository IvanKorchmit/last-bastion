using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeatherInfo : MonoBehaviour
{
    [SerializeField] private int index;
    public void Init(int index)
    {
        this.index += index;
    }
    private void Start()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";
        switch (Calendar.months[0].days[index].weather)
        {
            case Calendar.Day.WeatherType.None:
                text.text += "Clear - ";
                break;
            case Calendar.Day.WeatherType.Winter:
                text.text += "Winter - ";

                break;
            case Calendar.Day.WeatherType.Fog:
                text.text += "Fog - ";

                break;
            default:
                break;
        }
        text.text += Calendar.months[0].days[index].gameEvent?.Name ?? "None";
    }
}
