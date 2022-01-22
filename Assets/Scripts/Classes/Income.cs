using UnityEngine;
using LastBastion.Dialogue;

[CreateAssetMenu(fileName = "Income", menuName = "Events/News/Population Growth")]
public class Income : GameEvent
{
    public override void Launch()
    {
        Content.Choice[] Ok = new Content.Choice[] { new Content.Choice("Okay",
            () => {
                UIShow.CloseDialogue();
                return true;
            }, null, null)
        };


        Content mainContent = new Content(Ok, "We have received an income of 500 and population growth");
        HumanResourcesUtils.IncreaseHumanResources();
        HumanResourcesUtils.IncreaseHumanResources();
        HumanResourcesUtils.IncreaseHumanResources();
        HumanResourcesUtils.IncreaseHumanResources();


        DialogueUtils.Dialogue(mainContent);
    }
}
