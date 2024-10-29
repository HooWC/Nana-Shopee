using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Head { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Product_ID { get; set; }
        public string comment { get; set; }
    }
}
