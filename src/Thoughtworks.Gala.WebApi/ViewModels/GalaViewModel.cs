using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thoughtworks.Gala.WebApi.ViewModels
{
    public class GalaViewModel
    {
        [Required]
        public Guid GalaId { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1982, 2050)]
        public uint Year { get; set; }

        public IReadOnlyList<Guid> ProgramIds { get; set; } = new List<Guid>();

        public class Creation
        {
            [Required]
            [MaxLength(4000)]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Range(1982, 2050)]
            public uint Year { get; set; }

            [Required] 
            public IReadOnlyList<Guid> ProgramIds { get; set; } = new List<Guid>();

        }

        public class Edit : GalaViewModel
        {
        }
    }
}
