using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace OriginSolutions.Entities
{
    public partial class Session
    {
        [Key, StringLength(64, MinimumLength = 64)]
        public required string Token { get; set; }

        [StringLength(16, MinimumLength = 16)]
        public required string FK_Card { get; set; }
        [ForeignKey(nameof(FK_Card))]
        public required Card Card { get; set; }

        [GeneratedRegex(@"^[0-9A-Fa-f]{64}$")]
         public static partial Regex Token_Regex();
    }
}
