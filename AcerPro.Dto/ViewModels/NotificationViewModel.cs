using AcerPro.Dto.Base;
using AcerPro.Dto.Statics;
using System;

namespace AcerPro.Dto.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public int UserId { get; set; }

        public UserViewModel User { get; set; }

        public int ApplicationId { get; set; }

        public ApplicationViewModel Application { get; set; }

        public NotificationContent? NotificationContent { get; set; }

        public DateTime NotificationDate { get; set; } = DateTime.UtcNow;
    }
}
