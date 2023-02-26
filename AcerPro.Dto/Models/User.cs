using AcerPro.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace AcerPro.Dto.Models
{
    public class User : ModelBase
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
