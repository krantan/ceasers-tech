using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Business.Data
{
    [Table("Guest")]
    public class Guest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NameGiven { get; set; } = string.Empty;
        public string NameFamily { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmailHash { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public int IsActive { get; set; } = 1;
        public string CreatedTS { get; set; } = DateTime.Now.ToString();
        public string? ModifiedTS { get; set; }
    }
}
