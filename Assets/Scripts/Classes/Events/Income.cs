using UnityEngine;
using LastBastion.Dialogue;
using LastBastion.Economy;
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
                    DialogueContent.ChoiceButton[] Ok = new DialogueContent.ChoiceButton[] { new DialogueContent.ChoiceButton("Okay",
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
                    ShopUtils.GainMoney(500);

                    DialogueUtils.Dialogue(mainContent);
                }
            }
        }
    }
}