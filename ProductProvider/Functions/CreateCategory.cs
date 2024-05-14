using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductProvider.Contexts;
using ProductProvider.Entities;

namespace ProductProvider.Functions;

public class CreateCategory
{
    private readonly ILogger<CreateCategory> _logger;
    private readonly DataContext _context;

    public CreateCategory(ILogger<CreateCategory> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("CreateCategory")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var category = JsonConvert.DeserializeObject<Category>(body);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CreateCategory.Run() :: {ex.Message}");
        }
        return new BadRequestResult();
    }
}
