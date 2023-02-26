using AcerPro.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace AcerPro.Dto.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
