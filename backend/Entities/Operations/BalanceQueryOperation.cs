using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OriginSolutions.Entities.Operations
{
    public sealed class BalanceQueryOperation : OperationBase
    {
        /// <summary>
        /// Foreign Key for the associated Account
        /// </summary>
        [StringLength(22, MinimumLength = 22)]
        public required string FK_Account { get; set; }

        /// <summary>
        /// Navigation property for the associated Account
        /// </summary>
        [ForeignKey(nameof(FK_Account))]
        public required Account Account { get; set; }

        /// <summary>
        /// Foreign Key for the associated Card
        /// </summary>
        [StringLength(16, MinimumLength = 16)]
        public required string FK_Card { get; set; }
        
        /// <summary>
        /// Navigation property for the associated Card
        /// </summary>
        [ForeignKey(nameof(FK_Card))]
        public required Card Card { get; set; }

        /// <summary>
        /// The current balance of the account in cents
        /// </summary>
        public long Balance { get; set; }
    }
}
