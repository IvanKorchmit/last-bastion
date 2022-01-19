using UnityEngine;
[CreateAssetMenu(fileName = "Acid Rain", menuName = "Events/Weather/Acid Rain")]
public class AcidRain : GameEvent, IEventEndable
{
    public override void Launch()
    {
        WavesUtils.LightAnimator.SetBool("isAcid",true);
        WeatherUtils.status = WeatherUtils.Status.acid_rain;
    }
    public void End()
    {
        WavesUtils.LightAnimator.SetBool("isAcid",false);
        WeatherUtils.status = WeatherUtils.Status.none;
    }
}

