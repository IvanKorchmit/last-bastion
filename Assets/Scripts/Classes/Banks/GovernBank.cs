using UnityEngine;
using LastBastion.Dialogue;
using LastBastion.TimeSystem;
using LastBastion.Waves;
namespace LastBastion
{

    namespace Economy
    {

        [CreateAssetMenu(fileName ="New Government Bank", menuName = "Economy/Banks/Government")]
        public class GovernBank : BankBase
        {
            
            public override bool Decide(FinanceProfile profile, int currentAmount)
            {
                if (profile.Percentage > -0.3f && profile.Debts.Count <= 1)
                {
                    foreach (FinanceProfile.Debt d in profile.Debts)
                    {
                        if (d.Deadline >= 5)
                        {
                            continue;
                        }
                        if (d.Amount * d.Bank.Percentage > currentAmount)
                        {
                            return false;
                        }
                    }
                    return true;
                }

                return false;
            }

            public override void Offer()
            {
                DialogueContent success = DialogueUtils.GenerateDialogue($"Thank you, and you have received the funds. Please, return it at {debtDeadlineCheck} days.", DialogueUtils.OK);
                DialogueContent fail = DialogueUtils.GenerateDialogue("Unfortunately, according to your financial history and profile, we cannot accept this.", DialogueUtils.OK);
                DialogueContent deny = DialogueUtils.GenerateDialogue("Okay, come back later then.", DialogueUtils.OK);



                DialogueContent.Choice acceptOffer = DialogueUtils.CreateChoice("Accept offer and take 1000$", AcceptOffer, success, fail);
                DialogueContent.Choice declineOffer = DialogueUtils.CreateChoice("No, I won't accept this offer", () => { UIShow.CloseDialogue(); return true; }, deny);
                DialogueContent main = DialogueUtils.GenerateDialogue("Hello, thank you for choosing us.", acceptOffer, declineOffer);



                DialogueUtils.Dialogue(main);
            }
            public bool AcceptOffer()
            {
                bool decision = Decide(ShopUtils.Profile, ShopUtils.Money);
                // Debug.Log(ShopUtils.Profile);
                if (decision)
                {
                    ShopUtils.AddDebtAndReceiveMoney(this);
                }
                return decision;
            }

            public override void OnDeadlinePassed(int debt)
            {
                DialogueContent success = DialogueUtils.GenerateDialogue("Thank you for your cooperation!", DialogueUtils.OK);
                DialogueContent fail = DialogueUtils.GenerateDialogue("Looks like you do not have enough money in order to pay the loan. We have to take an action of taking your property!", DialogueUtils.OK);
                DialogueContent decline = DialogueUtils.GenerateDialogue("Unfortunately, I cannot leave this alone and since for non-payment, we should take your property away!", DialogueUtils.OK);
                DialogueContent.Choice payment = DialogueUtils.CreateChoice("Sure, I will pay debt", () =>
                {
                    int price = Mathf.RoundToInt((float)debt * percentage);
                    if (ShopUtils.CanAfford(price)) 
                    {
                        ShopUtils.Buy(price);
                        return true;
                    }
                    return false;
                },
                success,fail
                );
                DialogueContent.Choice denyChoice = DialogueUtils.CreateChoice("No, I will not pay the loan!", OnDenial, decline);
                DialogueContent main = DialogueUtils.GenerateDialogue($"Hello, according to your deadline, you have to give us our money back right now with a percentage of {100 - Mathf.RoundToInt(percentage * 100)}%." +
                    $"Meaning, with your initial debt of {debt}, you have to pay us {debt * percentage}",payment,denyChoice);
                DialogueUtils.Dialogue(main);
            }

            protected override bool OnDenial()
            {
                CloseRelationShips(this);
                Calendar.ChangeEvent(WavesUtils.WaveNumber + 7, loanPaymentDenialEvent);
                return true;
            }
        }
    }
}
