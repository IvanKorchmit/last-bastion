using UnityEngine;
using LastBastion.Dialogue;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Income", menuName = "Events/News/Population Growth")]
            public class Income : GameEvent
            {
                public override void Launch()
                {
                    DialogueContent.Choice[] Ok = new DialogueContent.Choice[] { new DialogueContent.Choice("Okay",
            () => {
                UIShow.CloseDialogue();
                return true;
            }, null, null)
        };


                    DialogueContent mainContent = new DialogueContent(Ok, "We have received an income of 500 and population growth");
                    HumanResourcesUtils.IncreaseHumanResources();
                    HumanResourcesUtils.IncreaseHumanResources();
                    HumanResourcesUtils.IncreaseHumanResources();
                    HumanResourcesUtils.IncreaseHumanResources();


                    DialogueUtils.Dialogue(mainContent);
                }
            }
        }
    }
}