using UnityEngine;
namespace LastBastion
{
    namespace Economy
    {
        public abstract class BankBase : ScriptableObject
        {
            [SerializeField] protected float percentage;
            [SerializeField] protected int minimalAmount;
            [SerializeField] protected int debtDeadlineCheck;
            public float Percentage => percentage;
            public abstract bool Decide(FinanceProfile profile, int currentAmount);
            public abstract void Offer();
            public abstract void OnDeadlinePassed(int debt);
            public abstract void Awake();
        }
    }
}
