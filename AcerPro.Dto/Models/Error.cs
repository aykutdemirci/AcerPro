using AcerPro.Dto.Base;

namespace AcerPro.Dto.Models
{
    public class Error : ModelBase
    {
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }
    }
}
