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
                return true;
            }, null, null)
        };
        Content yesContent = new Content(Ok, "Good, we are now good :)");
        Content noContent = new Content(Ok, "NO we are dying of starvation now :'(");
        Content.Choice noChoice = new Content.Choice("No",Deny, noContent, null);
        Content falseYesContent = new Content(new Content.Choice[] { noChoice }, "Looks like you do not have enough money for that :(");
        Content.Choice yesChoice = new Content.Choice("Yes", SpendMoney, yesContent, falseYesContent);
        
        Content.Choice[] choices = new Content.Choice[] { 
            yesChoice, 
            noChoice
        };

        Content mainContent = new Content(choices, "Hello, we are hungry");


        DialogueUtils.Dialogue(mainContent);
    }
    private bool SpendMoney()
    {
        if (ShopUtils.Money >= 500)
        {
            ShopUtils.Buy(100, null);
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
        for (int i = 0; i < Random.Range(2,3); i++)
        {
            if (!HumanResourcesUtils.TakeOne())
            {
                break;
            }
        }
        return true;
    }
}
public interface IEventEndable
{
    void End();
}