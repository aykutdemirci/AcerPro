using AcerPro.Dto.Base;
using AcerPro.Dto.Statics;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcerPro.Dto.Models
{
    public class Notification : ModelBase
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int ApplicationId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }

        public NotificationContent? NotificationContent { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
