using UnityEngine;
[CreateAssetMenu(fileName = "Acid Rain", menuName = "Events/Weather/Acid Rain")]
public class AcidRain : GameEvent, IEventEndable
{
    public override void Launch()
    {
        WavesUtils.LightAnimator.SetTrigger("AcidRain");
        WeatherUtils.status = WeatherUtils.Status.acid_rain;
    }
    public void End()
    {
        WavesUtils.LightAnimator.SetTrigger("AcidRain");
        WeatherUtils.status = WeatherUtils.Status.none;
    }
}
