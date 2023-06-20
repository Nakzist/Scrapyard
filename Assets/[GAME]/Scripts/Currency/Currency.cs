namespace _GAME_.Scripts.Currency
{
    public static class Currency
    {
        private static int _currency;
        
        public static void AddCurrency(int amount)
        {
            _currency += amount;
        }

        public static bool SpendCurrency(int amount)
        {
            var currentCurrency = _currency - amount;

            if (amount < 0)
            {
                return false;
            }

            _currency = currentCurrency;
            return true;
        }
        
        public static int GetCurrency()
        {
            return _currency;
        }
    }
}