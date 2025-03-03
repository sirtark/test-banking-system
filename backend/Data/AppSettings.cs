namespace OriginSolutions.Data{
    public class AppSettings
    {
        // Minimum withdrawal amount for cards (in cents)
        public int CardMinimalWithdraw { get; set; } = 100;

        // Minimum multiple for card withdrawals (in cents)
        public int CardWithdrawMinMultiple { get; set; } = 1000;

        // Initial balance of a new account
        public int InitialAccountBalance { get; set; } = 100000;

        // Prefix used for card numbers
        public string CardPrefix { get; set; } = "453912";

        // Maximum number of login attempts before blocking the card
        public int MaxLoginAttempts { get; set; } = 3;

        // Session timeout in minutes
        public int SessionTimeoutMinutes { get; set; } = 30;
    }

}