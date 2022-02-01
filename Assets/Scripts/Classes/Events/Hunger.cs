using UnityEngine;
using LastBastion.Dialogue;
using LastBastion.Economy;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "Hunger", menuName = "Events/Choice/Hunger")]
            public class Hunger : GameEvent
            {
                public override void Launch()
                {
                    DialogueContent.Choice[] Ok = new DialogueContent.Choice[] { new DialogueContent.Choice("Okay",
            () => {
                UIShow.CloseDialogue();
                return true;
            }, null, null)
        };
                    DialogueContent yesContent = new DialogueContent(Ok, "Good, we are now good :)");
                    DialogueContent noContent = new DialogueContent(Ok, "NO we are dying of starvation now :'(");
                    DialogueContent.Choice noChoice = new DialogueContent.Choice("No", Deny, noContent, null);
                    DialogueContent falseYesContent = new DialogueContent(new DialogueContent.Choice[] { noChoice }, "Looks like you do not have enough money for that :(");
                    DialogueContent.Choice yesChoice = new DialogueContent.Choice("Yes", SpendMoney, yesContent, falseYesContent);

                    DialogueContent.Choice[] choices = new DialogueContent.Choice[] {
            yesChoice,
            noChoice
        };

                    DialogueContent mainContent = new DialogueContent(choices, "Hello, we are hungry");


                    DialogueUtils.Dialogue(mainContent);
                }
                private bool SpendMoney()
                {
                    if (ShopUtils.CanAfford(100))
                    {
                        ShopUtils.Buy(100);
                        HumanResourcesUtils.IncreaseHumanResources();
                        HumanResourcesUtils.IncreaseChaos(-0.05f);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                private bool Deny()
                {
                    HumanResourcesUtils.IncreaseChaos(0.2f);
                    for (int i = 0; i < Random.Range(2, 3); i++)
                    {
                        if (!HumanResourcesUtils.TakeOne())
                        {
                            break;
                        }
                    }
                    return true;
                }
            }
        }
    }
}