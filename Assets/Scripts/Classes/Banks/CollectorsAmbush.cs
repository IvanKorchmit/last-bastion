using UnityEngine;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Collector Ambush", menuName = "Events/Loan/Collector Ambush")]
            public class CollectorsAmbush : GameEvent
            {
                [SerializeField] private GameObject[] enemies;
                [SerializeField] private int enemyQuantity;
                public override void Launch()
                {
                    SpawnerManager.Spawn(enemies, enemyQuantity);

                }
            }
        }
    }
}