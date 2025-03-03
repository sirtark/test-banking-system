namespace OriginSolutions.Services
{
    public sealed class CardGenService
    {
        static readonly Random random = new();
        public string GenerateCardNumber(string bin, int length = 16)
        {
            if (bin.Length >= length)
                throw new ArgumentException("The BIN (Bank Identification Number) must be shorter than the full card number.");
            int[] cardNumber = new int[length];
            for (int i = 0; i < bin.Length; i++)
                cardNumber[i] = bin[i] - '0';
            for (int i = bin.Length; i < length - 1; i++)
                cardNumber[i] = random.Next(0, 10);
            cardNumber[length - 1] = CalculateLuhnCheckDigit(cardNumber);
            return string.Concat(cardNumber.Select(d => d.ToString()));
        }
        private static int CalculateLuhnCheckDigit(int[] numbers)
        {
            int sum = 0;
            bool alternate = true;
            for (int i = numbers.Length - 2; i >= 0; i--)
            {
                int num = numbers[i];
                if (alternate)
                {
                    num *= 2;
                    if (num > 9) num -= 9;
                }
                sum += num;
                alternate = !alternate;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }
    }
}
