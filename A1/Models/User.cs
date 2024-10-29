using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string Hurl { get; set; }
        [Required]
        public string Role { get; set; }
        public double Money { get; set; }
        public double OutMoney { get; set; }
        public string City { get; set; }
        public string AddressZip { get; set; }
    }
}
