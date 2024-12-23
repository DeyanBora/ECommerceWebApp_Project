﻿using System.Text.Json;

namespace ECommerceWebApp.Api.Extensions.Middleware;

public static class HttpResponseExtensions
{
    public static void AddPaginationHeader(
        this HttpResponse response,
        int totalCount,
        int pageSize)
    {
        var paginationHeader = new
        {
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationHeader));
    }
}
