using System.ComponentModel.DataAnnotations;

namespace PFDASG1.Models
{
    public class User
    {
        public int Userid { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Pin is required.")]

        public string Pin { get; set; }
    }
}
