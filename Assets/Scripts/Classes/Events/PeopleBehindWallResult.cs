using UnityEngine;
using LastBastion.Dialogue;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "People Behind Wall Result", menuName = "Events/Choice/Results/People Outside")]
            public class PeopleBehindWallResult : GameEvent
            {
                [SerializeField] [TextArea] private string mainText;
                public override void Launch()
                {
                    HumanResourcesUtils.IncreaseChaos(-0.02f);
                    HumanResourcesUtils.IncreaseHumanResources(3);
                    DialogueContent main = DialogueUtils.GenerateDialogue(mainText, DialogueUtils.OK);
                    DialogueUtils.Dialogue(main);
                }
                
            }
           

        }
    }
}