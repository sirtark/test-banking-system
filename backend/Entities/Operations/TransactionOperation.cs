using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OriginSolutions.Entities.Operations
{
    public sealed class TransactionOperation : OperationBase, IValidatableObject
    {
        /// <summary>
        /// Foreign Key for the origin account
        /// </summary>
        [StringLength(22, MinimumLength = 22)]
        public required string FK_OriginAccount { get; set; }

        /// <summary>
        /// Navigation property for the origin account
        /// </summary>
        [ForeignKey(nameof(FK_OriginAccount))]
        public required Account OriginAccount { get; set; }

        /// <summary>
        /// Foreign Key for the destination account
        /// </summary>
        [StringLength(22, MinimumLength = 22)]
        public required string FK_DestinationnAccount { get; set; }

        /// <summary>
        /// Navigation property for the destination account
        /// </summary>
        [ForeignKey(nameof(FK_DestinationnAccount))]
        public required Account DestinationAccount { get; set; }

        /// <summary>
        /// Foreign Key for the authorizing card
        /// </summary>
        [StringLength(16, MinimumLength = 16)]
        public required string FK_AuthorCard { get; set; }

        /// <summary>
        /// Navigation property for the authorizing card
        /// </summary>
        [ForeignKey(nameof(FK_AuthorCard))]
        public required Card AuthorCard { get; set; }

        /// <summary>
        /// Amount transferred in cents
        /// </summary>
        public uint Amount { get; set; }

        /// <summary>
        /// New balance of the origin account after the transaction
        /// </summary>
        public long OriginNewBalance { get; set; }

        /// <summary>
        /// New balance of the destination account after the transaction
        /// </summary>
        public long DestinationNewBalance { get; set; }

        /// <summary>
        /// Validates the transaction to ensure the origin and destination accounts are different
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(FK_OriginAccount == FK_DestinationnAccount)
                yield return new ValidationResult($"Cannot receive a transaction to the same account. Account UBK: {FK_OriginAccount}");
        }
    }
}
