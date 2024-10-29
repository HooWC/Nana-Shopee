using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Tr
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string PaidTime { get; set; }//付费时间
        public string TransactionStatus { get; set; }//状态  pending
        public string CartList { get; set; }
        public string BillingAddress { get; set; }//  地址
        public double Tax { get; set; }
        public double GrandTotal { get; set; }
        public string TransactionId { get; set; }
    }
}
