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
        public string Name { get; set; }

        [Required]
        [Range(1982, 2050)]
        public uint Year { get; set; }

        public IReadOnlyList<Guid> ProgramIds { get; set; }

        public class Creation
        {
            [Required]
            [MaxLength(4000)]
            public string Name { get; set; }

            [Required]
            [Range(1982, 2050)]
            public uint Year { get; set; }

            [Required]
            public IReadOnlyList<Guid> ProgramIds { get; set; }

        }

        public class Edit : GalaViewModel
        {
        }
    }
}
