using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public double Price { get; set; }
        public double DelPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Page { get; set; }
        public string Product_ID { get; set; }
        public long P1 { get; set; }
        public long P2 { get; set; }
        public long P3 { get; set; }
        public long P4 { get; set; }
        public long BuyCount { get; set; }
    }
}
