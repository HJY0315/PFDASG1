namespace PFDASG1.Models
{
    public class Transactions
    {
        public int transactionId { get; set; }
        public int accountId { get; set; }
        public decimal amount { get; set; }
        public DateTime transactionDate { get; set; }
        public int receiverId { get; set; }
    }
}
