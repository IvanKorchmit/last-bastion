using UnityEngine;
using LastBastion.Dialogue;
namespace LastBastion
{

    namespace Economy
    {
        [CreateAssetMenu(fileName ="New Government Bank", menuName = "Economy/Banks/Government")]
        public class GovernBank : BankBase
        {

            public override bool Decide(FinanceProfile profile, int currentAmount)
            {
                if (profile.Percentage > 0.6f && profile.Debts.Count <= 1)
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
                DialogueContent.Choice accept = new DialogueContent.Choice("Accept the offer",AcceptOffer,null,null);
            }
            public bool AcceptOffer()
            {
                return Decide(ShopUtils.Profile, ShopUtils.Money);
            }
        }
    }
}
