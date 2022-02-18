using UnityEngine;
using LastBastion.TimeSystem.Events;
namespace LastBastion
{
    namespace Economy
    {
        public abstract class BankBase : ScriptableObject
        {
            public delegate void BankDelegate(BankBase bank);
            public static event BankDelegate OnBankClose;
            [SerializeField] protected float percentage;
            [SerializeField] protected int minimalAmount;
            [SerializeField] protected int debtDeadlineCheck;
            [SerializeField] protected GameEvent loanPaymentDenialEvent;
            public float Percentage => percentage;
            public int Deadline => debtDeadlineCheck;
            public abstract bool Decide(FinanceProfile profile, int currentAmount);
            public abstract void Offer();
            public abstract void OnDeadlinePassed(int debt);
            protected abstract bool OnDenial();
            protected void CloseRelationShips(BankBase bank)
            {
                OnBankClose?.Invoke(bank);
            }
        }
       
    }
}
