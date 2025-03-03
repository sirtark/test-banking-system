using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OriginSolutions.Data;

namespace OriginSolutions.Entities{
    [Index(nameof(FK_Card), nameof(OperationType))]
    public class OperationEntry
    {
        /// <summary>
        /// Unique identifier for the operation entry
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Date and time when the operation was performed
        /// </summary>
        public DateTime OperationDate { get; set; }
        
        /// <summary>
        /// Type of the operation (BalanceQuery, ATM_Withdraw, etc.)
        /// </summary>
        public OperationType OperationType { get; set; }

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
        /// Foreign Key for the associated Account
        /// </summary>
        [StringLength(22, MinimumLength = 22)]
        public required string FK_Account { get; set; }
        
        /// <summary>
        /// Navigation property for the associated Account
        /// </summary>
        [ForeignKey(nameof(FK_Account))]
        public required Account Account { get; set; }
    }

}