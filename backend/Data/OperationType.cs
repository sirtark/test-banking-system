namespace OriginSolutions.Data
{
    public enum OperationType : byte
    {
        /// <summary>
        /// Operation type for balance queries
        /// </summary>
        BalanceQuery,
        
        /// <summary>
        /// Operation type for ATM withdrawals
        /// </summary>
        ATM_Withdraw,
        
        /// <summary>
        /// Operation type for card-originated transactions
        /// </summary>
        CardOriginatedTransaction
    }

}