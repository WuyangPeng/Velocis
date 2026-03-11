namespace Game.Scripts.Main.Runtime.Utils
{
    public static class NumberFormatter
    {
        private const long Thousand = 1000;
        private const long Million = 1000000;
        private const long Billion = 1000000000;
        private const long Trillion = 1000000000000;

        public static string FormatNumber(long number)
        {
            return number switch
            {
                < Thousand => number.ToString(),
                < Million => (number / (double)Thousand).ToString("0.#") + "k",
                < Billion => (number / (double)Million).ToString("0.#") + "m",
                < Trillion => (number / (double)Billion).ToString("0.#") + "b",
                _ => (number / (double)Trillion).ToString("0.#") + "t"
            };
        }

        public static string FormatNumber(double number)
        {
            return number switch
            {
                < Thousand => number.ToString("0.#"),
                < Million => (number / Thousand).ToString("0.#") + "k",
                < Billion => (number / Million).ToString("0.#") + "m",
                < Trillion => (number / Billion).ToString("0.#") + "b",
                _ => (number / Trillion).ToString("0.#") + "t"
            };
        }
    }
}