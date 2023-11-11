namespace PFDASG1.Models
{
    public class CreditCard
    {
        public int creditCardId { get; set; }
        public int accountId { get; set; }
        public string cardNumber { get; set; }
        public DateTime expirationDate { get; set; }
        public string cvv { get; set; }
    }
}
