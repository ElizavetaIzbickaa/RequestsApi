using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsApi.Models
{
    [Table("clients")]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("is_blocked")]
        public bool IsBlocked { get; set; }
    }
}