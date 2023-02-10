
namespace Who_Owes_Money.Models
{
     public class ResultOfCalculation
    {
        public string UnderPay { get; set; }
        public string OverPay { get; set; }
        public decimal Amount { get; set; }
        public decimal Average { get; set; }
    }
}
