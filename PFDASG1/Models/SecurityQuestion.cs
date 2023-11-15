using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class SecurityQuestion
    {
        [Required(ErrorMessage = "Security question ID is required.")]
        [Display(Name = "Security Question ID")]
        public int SQId { get; set; }

        [Required(ErrorMessage = "Account ID is required.")]
        [Display(Name = "Account ID")]
        public int accountId { get; set; }

        [Required(ErrorMessage = "Question ID is required.")]
        [Display(Name = "Question ID")]
        public int questionId { get; set; }

        [Required(ErrorMessage = "Security question answer is required.")]
        [Display(Name = "Security Question Answer")]
        [StringLength(100, ErrorMessage = "Security question answer cannot exceed 100 characters.")]
        public string answer { get; set; }
    }
}
