using AcerPro.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace AcerPro.Dto.Models
{
    public class Application : ModelBase
    {
        [Url]
        [Required]
        public string Url { get; set; }

        [Required]
        [Range(minimum: 1, int.MaxValue)]
        public int TraceInterval { get; set; }
    }
}
