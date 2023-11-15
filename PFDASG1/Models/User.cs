using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class User
    {
        [Required(ErrorMessage = "User ID is required.")]
        [Display(Name = "User ID")] // Display name for Userid property
        public int Userid { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [Display(Name = "Full Name")] // Display name for Name property
        public string Name { get; set; }

        [Required(ErrorMessage = "Access code is required.")]
        [StringLength(7, ErrorMessage = "Access code must be 7 characters long.")]
        [Display(Name = "Access Code")] // Display name for AccessCode property
        public string AccessCode { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+65\d{8}$", ErrorMessage = "Phone number must be in the format +65XXXXXXXX.")]
        [Display(Name = "Phone Number")] // Display name for phoneNumber property
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Pin is required.")]
        [StringLength(6, ErrorMessage = "Pin must be 6 characters long.")]
        [Display(Name = "PIN")] // Display name for Pin property
        public string Pin { get; set; }

        [Required(ErrorMessage = "NRIC is required.")]
        [StringLength(9, ErrorMessage = "NRIC must be 9 characters long.")]
        [RegularExpression(@"^[S|T]\d{7}[A-Z]$", ErrorMessage = "NRIC must be in the format S/T followed by 7 digits and an alphabet.")]
        [Display(Name = "NRIC")] // Display name for NRIC property
        public string NRIC { get; set; }
    }
}
