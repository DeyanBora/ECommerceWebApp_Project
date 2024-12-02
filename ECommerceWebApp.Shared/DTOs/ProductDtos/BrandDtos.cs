﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Shared.DTOs.ProductDtos
{
    public record CreateBrandDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
    }

    public record UpdateBrandDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
    }
}
