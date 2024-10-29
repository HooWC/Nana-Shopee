using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Img { get; set; }
        public string ProductName { get; set; }
        public string Product_ID { get; set; }
        public int Quantity { get; set; }
        public int LastQuantity { get; set; }
        public double Product_Price { get; set; }
        public bool Buy { get; set; }
    }
}
