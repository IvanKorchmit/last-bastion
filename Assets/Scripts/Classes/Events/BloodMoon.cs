using UnityEngine;
using LastBastion.Waves;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Blood Moon", menuName = "Events/Weather/Blood Moon")]
            public class BloodMoon : GameEvent, IEventEndable
            {
                public enum BloodMoonStatus
                {
                    Begin, End, Update
                }
                public static event System.Action<BloodMoonStatus> OnBloodMoonChange;
                public override void Launch()
                {
                    OnBloodMoonChange?.Invoke(BloodMoonStatus.Begin);
                    AIBase.OnEnemySpawn += AIBase_OnEnemySpawn;
                }

                private void AIBase_OnEnemySpawn()
                {
                    OnBloodMoonChange?.Invoke(BloodMoonStatus.Update);
                }

                public void End()
                {
                    OnBloodMoonChange?.Invoke(BloodMoonStatus.End);
                }
            }
        }
    }
}