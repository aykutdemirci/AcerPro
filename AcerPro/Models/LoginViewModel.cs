using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcerPro.Models
{
    public class LoginViewModel
    {
        [Required]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Email value was not the expected format")]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}