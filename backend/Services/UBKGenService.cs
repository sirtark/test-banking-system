namespace OriginSolutions.Services
{
    public sealed class UBKGenService{
        static readonly Random random = new();
        public string GenerateUBK()
        {
            string entity = GenerateRandomNumber(7);
            int verificationEntity = CalculateVerificador(entity);
            string accountNumber = GenerateRandomNumber(13);
            int verificationAccount = CalculateVerificador(accountNumber);
            return $"{entity}{verificationEntity}{accountNumber}{verificationAccount}";
        }
        static string GenerateRandomNumber(int length)
            => string.Concat(Enumerable.Range(0, length).Select(_ => random.Next(0, 10).ToString()));
        private static int CalculateVerificador(string number)
        {
            int[] weights = [3, 1, 7, 9];
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
                sum += (number[i] - '0') * weights[i % weights.Length];
            int remainder = sum % 10;
            return remainder == 0 ? 0 : 10 - remainder;
        }
    }
}
