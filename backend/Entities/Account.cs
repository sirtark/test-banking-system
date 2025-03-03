using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace OriginSolutions.Entities
{
    [Index(nameof(Alias), IsUnique = true), Index(nameof(UBK), nameof(Alias), nameof(NID), IsUnique = true)]
    public partial class Account {
        /// <summary>
        /// Owner's name from the account
        /// </summary>
        [StringLength(70)] 
        public required string Owner { get; set; }
        
        /// <summary>
        /// National Identity Document
        /// </summary>
        public uint NID { get; set; }
        
        /// <summary>
        /// Uniform Bank Key
        /// </summary>
        [Key, StringLength(22, MinimumLength = 22)] 
        public required string UBK { get; set; }
        
        /// <summary>
        /// Current Account Balance in cents
        /// </summary>
        public long Balance { get; set; }
        
        /// <summary>
        /// Account Alias to receive transactions
        /// </summary>
        [StringLength(20, MinimumLength = 6)] 
        public required string Alias { get; set; }

        /// <summary>
        /// Collection of Cards associated with the account
        /// </summary>
        public required ICollection<Card> Cards { get; set; }

        [GeneratedRegex(@"^\d{22}$")]
        public static partial Regex UBK_Regex();

        [GeneratedRegex(@"^(?!\.)[a-zA-Z](?!.*\.\.)[a-zA-Z.]{5,19}$")]
        public static partial Regex Alias_Regex();

        [GeneratedRegex(@"^\d{8}$")]
        public static partial Regex Nid_Regex();

        [GeneratedRegex(@"^([A-Za-z]+(?: [A-Za-z]+){0,2}) ([A-Za-z]+(?: [A-Za-z]+){0,2})$")]
        public static partial Regex Name_Regex();
    }
}