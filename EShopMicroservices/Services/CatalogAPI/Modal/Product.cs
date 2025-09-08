using System;

namespace CatalogAPI.Modal
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public List<string> Category { get; set; } = new List<string>();
        public string ImageUrl { get; set; } = default!;
        //public string Quantity { get; set; } = default!;
    }
}
