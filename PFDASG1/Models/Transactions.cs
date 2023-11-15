using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PFDASG1.Models
{
    public class Transactions
    {
        [Required(ErrorMessage = "Transaction ID is required.")]
        [Display(Name = "Transaction ID")]
        public int TransactionId{ get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Account ID is required.")]
        [Display(Name = "Account ID")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Display(Name = "Amount")]
        [Range(-9999999999.99, 9999999999.99, ErrorMessage = "Amount must be between -9999999999.99 and 9999999999.99.")] public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Sender ID is required.")]
        [Display(Name = "Sender/Recipient ID")]
        public int ReceipientOrSenderID { get; set; }
    }
}
