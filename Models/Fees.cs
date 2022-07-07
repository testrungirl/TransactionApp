namespace TransactionApp.Models
{
    public class FeeObjects
    {
        public decimal minAmount { get; set; }
        public decimal maxAmount { get; set; }
        public decimal feeAmount { get; set; }
    }

    public class FeeClass
    {
        public List<FeeObjects> Fees { get; set; } = new List<FeeObjects>();
    }
}
