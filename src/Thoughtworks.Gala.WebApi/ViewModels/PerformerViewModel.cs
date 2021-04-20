using System;
using System.ComponentModel.DataAnnotations;

namespace Thoughtworks.Gala.WebApi.ViewModels
{
    public class PerformerViewModel
    {
        [Required]
        public Guid PerformerId { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Name { get; set; } = string.Empty;

        public class Creation
        {
            [Required]
            [MaxLength(4000)]
            public string Name { get; set; } = string.Empty;
        }

        public class Edit : PerformerViewModel
        {
        }
    }
}