using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Shared.DTOs.Search;

public record CreateElasticProductDto(string Name, string Description, decimal Price, int Stock);

public record UpdateElasticProductDto(Guid Id, string Name, string Description, decimal Price, int Stock);