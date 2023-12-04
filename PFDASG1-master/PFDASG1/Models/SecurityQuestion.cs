using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class SecurityQuestion
    {
        [Display(Name = "Security Question")]
        public string SQ { get; set; }

        [Display(Name = "Security Question Answer")]
        [StringLength(100, ErrorMessage = "Security question answer cannot exceed 100 characters.")]
        public string answer { get; set; }

    }
}
