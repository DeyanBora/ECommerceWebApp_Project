using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Shared.DTOs.ProductDtos
{
    public record CreateManufacturerDto
    {
        public string Name { get; init; } = string.Empty;
        public string ContactInfo { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
    }
    public record UpdateManufacturerDto
    {
        public string Name { get; init; } = string.Empty;
        public string ContactInfo { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
    }
}
