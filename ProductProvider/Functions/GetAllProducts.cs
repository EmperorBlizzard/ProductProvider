using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductProvider.Contexts;
using ProductProvider.Entities;
using System.Drawing.Printing;

namespace ProductProvider.Functions
{
    public class GetAllProducts
    {
        private readonly ILogger<GetAllProducts> _logger;
        private readonly DataContext _context;

        public GetAllProducts(ILogger<GetAllProducts> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("GetAllProducts")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                var category = req.Query["category"].ToString();
                var color = req.Query["color"].ToString();
                var size = req.Query["size"].ToString();
                var batch = req.Query["batch"].ToString();

                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(batch))
                {
                    query = query.Where(x => x.BatchNumber == batch);
                }

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(x => x.Categories.Contains(category!));
                }

                if (!string.IsNullOrEmpty(color))
                {
                    query = query.Where(x => x.Color == color);
                }

                if (!string.IsNullOrEmpty(size))
                {
                    query = query.Where(x => x.Size == size);
                }

                

                var products = await query.ToListAsync();

                return new OkObjectResult(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : GetAllProducts.Run() :: {ex.Message}");
            }
            return new NotFoundResult();
        }
    }
}
