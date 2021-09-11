using UnityEngine;
using Dialogue;

[CreateAssetMenu(fileName = "Hunger", menuName = "Events/Choice/Hunger")]
public class Hunger : GameEvent
{
    public override void Launch()
    {
        Content.Choice[] Ok = new Content.Choice[] { new Content.Choice("Okay",
            () => { 
                DialogueUtils.CloseDIalogue(); 
            }) 
        };
        Content.Choice yesChoice = new Content.Choice("Yes", SpendMoney);
        Content.Choice noChoice = new Content.Choice("No",Deny);
        Content.Choice[] choices = new Content.Choice[] { 
            yesChoice, 
            noChoice
        };
        Content yesContent = new Content(Ok, "Good, we are now good :)", null);
        Content noContent = new Content(Ok, "NO we are dying of starvation now :'(", null);

        Content[] nestedContents = new Content[] { yesContent, noContent };

        Content mainContent = new Content(choices, "Hello, we are hungry", nestedContents);
    }
    private void SpendMoney()
    {
        ShopUtils.Buy(100, null);
    }
    private void Deny()
    {
        HumanResourcesUtils.IncreaseChaos(0.2f);
        for (int i = 0; i < Random.Range(2,3); i++)
        {
            if (!HumanResourcesUtils.TakeOne())
            {
                break;
            }
        }
    }
}
