using AcerPro.Dto.Base;
using System;

namespace AcerPro.Dto.ViewModels
{
    public class ErrorViewModel : ViewModelBase
    {
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }
    }
}
