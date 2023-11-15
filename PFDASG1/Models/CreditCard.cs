using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class CreditCard
    {
        [Required(ErrorMessage = "Credit card ID is required.")]
        [Display(Name = "Credit Card ID")]
        public int creditCardId { get; set; }

        [Required(ErrorMessage = "Account ID is required.")]
        [Display(Name = "Account ID")]
        public int accountId { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [Display(Name = "Card Number")]
        [StringLength(20, ErrorMessage = "Card number cannot exceed 20 characters.")]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Card number must be 16 digits long.")]
        public string cardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [Display(Name = "Expiration Date")]
        //[DateRange(DateTime.Today, DateTime.Today.AddYears(10), ErrorMessage = "Expiration date must be within the next 10 years.")]
        public DateTime expirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [Display(Name = "CVV")]
        [StringLength(3, ErrorMessage = "CVV must be 3 characters long.")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVV must be 3 digits long.")]
        public string cvv { get; set; }
    }
}
