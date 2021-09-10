using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Acid Rain", menuName = "Events/Weather/Acid Rain")]
public class AcidRain : GameEvent
{
    public override void Launch()
    {
        WavesUtils.LightAnimator.SetTrigger("AcidRain");
        WeatherUtils.status = WeatherUtils.Status.acid_rain;
    }
}