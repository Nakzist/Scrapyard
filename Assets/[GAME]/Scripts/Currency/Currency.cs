namespace _GAME_.Scripts.Currency
{
    public static class Currency
    {
        private static int _currency;
        
        public static bool CleaverUnlocked;
        public static bool KanaboUnlocked;
        public static bool KatanaUnlocked = true;
        public static bool SpearUnlocked;
        
        public static bool DoubleBarrelUnlocked;
        public static bool GrenadeLauncherUnlocked;
        public static bool MinigunUnlocked;
        public static bool RevolverUnlocked = true;
        
        public static void AddCurrency(int amount)
        {
            _currency += amount;
        }

        public static bool SpendCurrency(int amount)
        {
            var currentCurrency = _currency - amount;

            if (currentCurrency < 0)
            {
                currentCurrency = 0;
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