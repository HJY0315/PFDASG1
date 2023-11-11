namespace PFDASG1.Models
{
    public class Account
    {
        public int accountId { get; set; }
        public string accountType { get; set; }
        public decimal accountBalance { get; set; }
        public int userId { get; set; }
        public string NRIC { get; set; }
    }
}
