using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.Economy;
using LastBastion.Waves;
public class BankManager : MonoBehaviour
{
    [SerializeField] private BankBase[] banks;
    public void OfferBank(int index)
    {
        banks[0].Offer();
    }
}
