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
                    DialogueContent success = DialogueUtils.GenerateDialogue("Thank you! Now we can fight for you!", DialogueUtils.OK);
                    DialogueContent fail = DialogueUtils.GenerateDialogue("Looks like you do not have enough money for that... Please, we are dying of starvation!", DialogueUtils.OK);
                    DialogueContent deny = DialogueUtils.GenerateDialogue("I am really sorry to hear that... Please, we are dying of starvation!", DialogueUtils.OK);
                    DialogueContent.Choice yes = DialogueUtils.CreateChoiceButton("Sure, I will spend 100$ for that!", SpendMoney, success ,fail);
                    DialogueContent.Choice no = DialogueUtils.CreateChoiceButton("No, I will decline this request!", Deny,deny);
                    DialogueContent main = DialogueUtils.GenerateDialogue("Hello, we are hungry and we acquire some ration in order to survive!",yes,no);

                    DialogueUtils.Dialogue(main);


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