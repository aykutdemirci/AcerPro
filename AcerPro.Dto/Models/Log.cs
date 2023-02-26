using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AcerPro.Dto.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserEmail { get; set; }

        public int? EntitiyId { get; set; }

        [Required]
        public string EntityName { get; set; }

        [Required]
        public string BeforeChange { get; set; }

        public EntityState EntityState { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
