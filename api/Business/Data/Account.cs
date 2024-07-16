using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Business.Data
{
    [Table("Account")]
    [Index(nameof(Username), IsUnique=true)]
    public class Account
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set;} = string.Empty;
        public string CreatedTS { get; set; } = DateTime.Now.ToString();
        public string? ModifiedTS { get; set; }
    }
}
