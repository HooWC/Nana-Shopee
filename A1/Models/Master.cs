using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Master
    {
        [Key]
        public int Id { get; set; }
        public string Mas { get; set; }
        public string Info { get; set; }
        public string Img { get; set; }
    }
}
