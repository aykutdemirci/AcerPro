using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcerPro.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        public string Name { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}