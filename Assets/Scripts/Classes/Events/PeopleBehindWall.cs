using UnityEngine;
using LastBastion.Dialogue;
using LastBastion.Economy;
using LastBastion.Waves;
namespace LastBastion
{
    namespace TimeSystem
    {
        namespace Events
        {
            [CreateAssetMenu(fileName = "People Behind Wall", menuName = "Events/Choice/People Outside")]
            public class PeopleBehindWall : GameEvent
            {
                [SerializeField] GameEvent results;
                [SerializeField] [TextArea] private string mainText;
                [SerializeField] [TextArea] private string denyRescueText;
                [SerializeField] [TextArea] private string acceptRescueText;
                [SerializeField] private string accept;
                [SerializeField] private string deny;
                public override void Launch()
                {
                    DialogueContent success = DialogueUtils.GenerateDialogue(acceptRescueText, DialogueUtils.OK);
                    DialogueContent fail = DialogueUtils.GenerateDialogue(denyRescueText, DialogueUtils.CreateChoice("Okay", Deny, null));
                    DialogueContent.Choice rescue = DialogueUtils.CreateChoice(accept, Rescue,success,fail);
                    DialogueContent.Choice deny = DialogueUtils.CreateChoice(this.deny, Deny, fail);
                    DialogueContent main = DialogueUtils.GenerateDialogue(mainText, rescue,deny);
                    DialogueUtils.Dialogue(main);
                }
                private bool Rescue()
                {
                    if (HumanResourcesUtils.HumanResources >= 4 && ShopUtils.ResourceA >= 10)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            HumanResourcesUtils.TakeOne();
                        }
                        ShopUtils.Buy(10, ShopUtils.ResourceType.A);
                        HumanResourcesUtils.IncreaseChaos(-0.02f);
                        Calendar.ChangeEvent(WavesUtils.WaveNumber + 2, results);
                        return true;
                    }
                    return false;
                }
                private bool Deny()
                {
                    HumanResourcesUtils.IncreaseChaos(0.03f);
                    UIShow.CloseDialogue();
                    return true;
                }
            }
           

        }
    }
}