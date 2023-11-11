namespace PFDASG1.Models
{
    public class SecurityQuestion
    {
        public int SQId { get; set; }
        public int accountID { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
