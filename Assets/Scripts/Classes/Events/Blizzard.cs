using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.TimeSystem.Events;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Blizzard", menuName = "Events/Weather/Blizzard")]
            public class Blizzard : GameEvent, IEventEndable
            {

                public static event System.Action<bool> OnBlizzard;

                public void End()
                {
                    OnBlizzard?.Invoke(false);
                    AIBase.OnEnemySpawn -= AIBase_OnEnemySpawn;
                }

                public override void Launch()
                {
                    OnBlizzard?.Invoke(true);
                    AIBase.OnEnemySpawn += AIBase_OnEnemySpawn;
                }

                private void AIBase_OnEnemySpawn()
                {
                    OnBlizzard?.Invoke(true);
                }
            }
        }
    }
}