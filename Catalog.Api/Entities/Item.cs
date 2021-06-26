using System;

namespace Catalog.Entities
{
    //RecordTypes classes have better support immutable object
    public record Item
    {
        public Guid Id { get; init; } //init addition to .net 5, to create immutable properties and better than private set and no need to create constructor.
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
