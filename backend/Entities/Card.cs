using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace OriginSolutions.Entities{
    public partial class Card {
        /// <summary>
        /// Owner's name from the card, it's not the same as the Account's Owner
        /// </summary>
        [StringLength(70)] 
        public required string Owner { get; set; }
        
        /// <summary>
        /// National Identity Document
        /// </summary>
        public uint NID { get; set; }

        /// <summary>
        /// Card number, a 16-digit number
        /// </summary>
        [StringLength(16, MinimumLength = 16), Key] 
        public required string Number { get; set; }
        
        /// <summary>
        /// Number of failed login attempts
        /// </summary>
        public byte LoginFails { get; set; }
        
        /// <summary>
        /// Indicates whether the card is blocked
        /// </summary>
        public bool Blocked { get; set; }
        
        /// <summary>
        /// Card's PIN (hashed as SHA256)
        /// </summary>
        [StringLength(64, MinimumLength = 64)]
        public required string Pin { get; set; }

        /// <summary>
        /// Card's CVV code
        /// </summary>
        [StringLength(4, MinimumLength = 3)]
        public required string Cvv { get; set; }

        /// <summary>
        /// Card expiration date
        /// </summary>
        public DateOnly Expire { get; set; }
        
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

        [GeneratedRegex(@"^\d{16}$")]
        public static partial Regex Number_Regex();
        
        [GeneratedRegex(@"^\d{4}$")]
        public static partial Regex Pin_Regex();
    }
}