using System;
using System.ComponentModel.DataAnnotations;

namespace Thoughtworks.Gala.WebApi.ViewModels
{
    public class ProgramViewModel
    {
        [Required] 
        public Guid ProgramId { get; set; }

        [Required] 
        [MaxLength(4000)]
        public string Name { get; set; } = string.Empty;

        public class Creation
        {
            [Required] 
            [MaxLength(4000)] 
            public string Name { get; set; } = string.Empty;
        }

        public class Edit : ProgramViewModel
        {
        }
    }
}