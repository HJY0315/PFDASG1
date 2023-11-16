using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class CreditCard
    {
        [Display(Name = "Credit Card ID")]
        public int creditCardId { get; set; }

        [Display(Name = "Account ID")]
        public int accountId { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [Display(Name = "Card Number1")]
        [StringLength(16, ErrorMessage = "Card number cannot exceed 4 characters.")]
        public string cardNumber1 { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [Display(Name = "Card Number2")]
        [StringLength(16, ErrorMessage = "Card number cannot exceed 4 characters.")]
        public string cardNumber2 { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [Display(Name = "Expiration Month")]
        public int expirationMonth { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [Display(Name = "Expiration Year")]
        public int expirationYear { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [Display(Name = "CVV")]
        [StringLength(3, ErrorMessage = "CVV must be 3 characters long.")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVV must be 3 digits long.")]
        public string cvv { get; set; }

        [Required(ErrorMessage = "Card Holder Name is required")]
        [Display(Name = "Card Holder Name")]
        [StringLength(25, ErrorMessage = "Card Holder Name cannot exceed 25 characters")]
        public string cardHolderName { get; set; }

        [Display(Name = "Card Status")]
        public string cardStatus { get; set; }

        [Display(Name = "Security Question")]
        public string securityQuestion { get; set; }

        [Display(Name = "Answer")]
        public string answer { get; set; }

        public SecurityQuestion SQ { get; set; }
    }
}
