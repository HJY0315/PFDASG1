using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PFDASG1.Models
{
    public class Transactions
    {
        public int transactionId { get; set; }
        public int accountId { get; set; }
        public string description {get; set;}

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Please enter a City!")]
        public decimal amount { get; set; }
        public DateTime transactionDate { get; set; }
        public int receiverId { get; set; }
    }
}
