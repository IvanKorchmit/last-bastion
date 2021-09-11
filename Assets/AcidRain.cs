using UnityEngine;
[CreateAssetMenu(fileName = "Acid Rain", menuName = "Events/Weather/Acid Rain")]
public class AcidRain : GameEvent
{
    public override void Launch()
    {
        WavesUtils.LightAnimator.SetTrigger("AcidRain");
        WeatherUtils.status = WeatherUtils.Status.acid_rain;
    }
    public override void End()
    {
        WavesUtils.LightAnimator.SetTrigger("AcidRain");
        WeatherUtils.status = WeatherUtils.Status.none;
    }
}
