﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using NotSupportedException = Thoughtworks.Gala.WebApi.Exceptions.NotSupportedException;

namespace Thoughtworks.Gala.WebApi.Entities
{
    [DynamoDBTable("Galas")]
    public class GalaEntity : IEntity<Guid>, IAssignableEntity<Guid>
    {
        [DynamoDBHashKey("GalaId", Converter = typeof(GuidConverter))]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public uint Year { get; set; }

        [DynamoDBProperty("Programs")]
        public IReadOnlyList<Guid> ProgramIds { get; set; }

        public Task AssignFromAsync(IEntity<Guid> other)
        {
            if (!(other is GalaEntity source))
            {
                throw new NotSupportedException(nameof(GalaEntity));
            }

            // ignore id
            Name = source.Name;
            Year = source.Year;
            ProgramIds = source.ProgramIds?.ToList().AsReadOnly();

            return Task.CompletedTask;
        }
    }
}