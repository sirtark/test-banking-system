using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OriginSolutions.Entities
{
    public abstract class OperationBase
    {
        /// <summary>
        /// Unique identifier for the operation base
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign Key for the related OperationEntry
        /// </summary>
        public int FK_Entry { get; set; }
        
        /// <summary>
        /// Navigation property for the related OperationEntry
        /// </summary>
        [ForeignKey(nameof(FK_Entry))]
        public required OperationEntry Entry { get; set; }
    }
}
