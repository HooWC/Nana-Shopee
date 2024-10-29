using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Src { get; set; }
        public string Img { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
        public int Product_ID { get; set; }
    }
}
