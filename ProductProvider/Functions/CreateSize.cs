using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductProvider.Contexts;
using ProductProvider.Entities;

namespace ProductProvider.Functions;

public class CreateSize
{
    private readonly ILogger<CreateSize> _logger;
    private readonly DataContext _context;

    public CreateSize(ILogger<CreateSize> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("CreateSize")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var size = JsonConvert.DeserializeObject<Size>(body);

            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CreateSize.Run() :: {ex.Message}");
        }
        return new BadRequestResult();
    }
}
