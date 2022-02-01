using UnityEngine;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Acid Rain", menuName = "Events/Weather/Acid Rain")]
            public class AcidRain : GameEvent, IEventEndable
            {
                public enum AcidRainStatusType
                {
                    Begin, End
                }
                public static event System.Action<AcidRainStatusType> OnAcidRainChange;
                public override void Launch()
                {
                    OnAcidRainChange?.Invoke(AcidRainStatusType.Begin);
                    WeatherUtils.status = WeatherUtils.Status.AcidRain;
                }

                public void End()
                {
                    OnAcidRainChange?.Invoke(AcidRainStatusType.End);

                    WeatherUtils.status = WeatherUtils.Status.None;
                }
            }
        }
    }
}