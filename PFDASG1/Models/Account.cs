using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Account ID is required.")]
        [Display(Name = "Account ID")]
        public int accountId { get; set; }

        [Required(ErrorMessage = "Account type is required.")]
        [Display(Name = "Account Type")]
        [StringLength(20, ErrorMessage = "Account type cannot exceed 20 characters.")]
        public string accountType { get; set; }

        [Required(ErrorMessage = "Account balance is required.")]
        [Display(Name = "Account Balance")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Account balance must be a positive number.")]
        public decimal accountBalance { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [Display(Name = "User ID")]
        public int userId { get; set; }

        [Required(ErrorMessage = "Daily Limit is required.")]
        [Display(Name = "Daily Limit")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Account balance must be a positive number.")]
        public decimal dailyLimit { get; set; }
    }
}
