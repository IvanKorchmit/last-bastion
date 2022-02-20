using UnityEngine;
using LastBastion.Dialogue;
using LastBastion.TimeSystem;
using LastBastion.Waves;
namespace LastBastion
{
    namespace Economy
    {
        [CreateAssetMenu(fileName = "New Government Bank", menuName = "Economy/Banks/Government")]
        public class GovernBank : BankBase
        {
            public override bool Decide(FinanceProfile profile, int currentAmount)
            {
                if (profile.Percentage > -0.3f && profile.Debts.Count <= 1)
                {
                    foreach (FinanceProfile.Debt d in profile.Debts)
                    {
                        if (d.Bank == this) return false;
                    }
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

                DialogueContent.ChoiceSlider amount = DialogueUtils.CreateSlider("Amount: {0}$", minimalAmount, minimalAmount * 2);

                DialogueContent.Choice acceptOffer = DialogueUtils.CreateChoiceButton("Accept offer and submit", ()=>
                {
                    bool decision = Decide(ShopUtils.Profile, ShopUtils.Money);
                    if (decision)
                    {
                        ShopUtils.AddDebtAndReceiveMoney(this, amount.Value);
                    }
                    return decision;

                }, success, fail);
                DialogueContent.Choice declineOffer = DialogueUtils.CreateChoiceButton("No, I won't accept this offer", () => { UIShow.CloseDialogue(); return true; }, deny);
                DialogueContent main = DialogueUtils.GenerateDialogue("Hello, thank you for choosing us.", acceptOffer, declineOffer, amount);


                DialogueUtils.Dialogue(main);
            }
            public override void OnDeadlinePassed(int debt)
            {
                DialogueContent success = DialogueUtils.GenerateDialogue("Thank you for your cooperation!", DialogueUtils.OK);
                DialogueContent fail = DialogueUtils.GenerateDialogue("Looks like you do not have enough money in order to pay the loan. We have to take an action of taking your property!", DialogueUtils.OK);
                DialogueContent decline = DialogueUtils.GenerateDialogue("Unfortunately, I cannot leave this alone and since for non-payment, we should take your property away!", DialogueUtils.OK);
                DialogueContent.Choice payment = DialogueUtils.CreateChoiceButton("Sure, I will pay debt", () =>
                {
                    int price = Mathf.RoundToInt((float)debt * percentage);
                    if (ShopUtils.CanAfford(price))
                    {
                        ShopUtils.Buy(price);
                        ShopUtils.RemoveDebts(this);
                        return true;
                    }
                    return false;
                },
                success, fail
                );
                DialogueContent.Choice denyChoice = DialogueUtils.CreateChoiceButton("No, I will not pay the loan!", OnDenial, decline);
                DialogueContent main = DialogueUtils.GenerateDialogue($"Hello, according to your deadline, you have to give us our money back right now with a percentage of {100 - Mathf.RoundToInt(percentage * 100)}%." +
                    $"Meaning, with your initial debt of {debt}, you have to pay us {debt * percentage}", payment, denyChoice);
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
