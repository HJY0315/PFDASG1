using System.ComponentModel.DataAnnotations;
using System.Transactions;
using System.Xml.Linq;

namespace PFDASG1.Models
{
    public class TransactionViewModel
    {
        
        public User receipient { get; set; }

        public int senderID { get; set; }

        public string phoneNumber { get; set; }
        
        public int TransactionId { get; set; }

        
        public int AccountId { get; set; }

        
        public int RecipientID { get; set; }

        
        public int SenderID { get; set; }


        
        public decimal Amount { get; set; }

        
        public string? Description { get; set; }

        public DateTime TransactionDate { get; set; }

    }
}
