using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductProvider.Contexts;
using ProductProvider.Entities;

namespace ProductProvider.Functions;

public class CreateProduct
{
    private readonly ILogger<CreateProduct> _logger;
    private readonly DataContext _context;

    public CreateProduct(ILogger<CreateProduct> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("CreateProduct")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(body);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CreateColor.Run() :: {ex.Message}");
        }
        return new BadRequestResult();
    }
}
