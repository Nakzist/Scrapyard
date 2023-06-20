using UnityEngine;
using static _GAME_.Scripts.Currency.Currency;

namespace _GAME_.Scripts.Currency
{
    public class CurrencyHudController : MonoBehaviour
    {
        
        
        public void BuyItem()
        {
            SpendCurrency(1);
        }
    }
}