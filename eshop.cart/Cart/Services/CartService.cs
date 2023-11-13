using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Cart.Services;

public class CartService : ICartService
{
    private readonly ILogger<CartService> _logger;
    private readonly string _userEmail;
    private readonly IDistributedCache _redisStorage;

    public CartService(ILogger<CartService> logger, TokenService tokenService, IDistributedCache redisStorage)
    {
        _logger = logger;
        _redisStorage = redisStorage;
        _userEmail = tokenService.GetEmail();
    }

    public async Task<List<string>> GetAll()
    {
        _logger.LogInformation("Get All");
        try
        {
            var data = await _redisStorage.GetStringAsync(_userEmail);
            if (data != null)
            {
                var productCodes = JsonSerializer.Deserialize<List<string>>(data);
                if (productCodes != null)
                {
                    return productCodes;
                }
                else
                {
                    await _redisStorage.RemoveAsync(_userEmail);
                }
            }

            return new List<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Something went wrong");
        }
    }

    public async Task Save(string productCode)
    {
        try
        {
            var data = await _redisStorage.GetStringAsync(_userEmail) ?? "";
            
            List<string> productCodes = null;
            if (!string.IsNullOrEmpty(data))
                productCodes = JsonSerializer.Deserialize<List<string>>(data);

            if (productCodes == null)
                productCodes = new List<string>();

            productCodes.Add(productCode);

            var userString = JsonSerializer.Serialize(productCodes);
            await _redisStorage.SetStringAsync(_userEmail, userString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Something went wrong");
        }
    }

    public async Task Delete(string productCode)
    {
        try
        {
            var data = await _redisStorage.GetStringAsync(_userEmail);
            if (data != null)
            {
                var productCodes = JsonSerializer.Deserialize<List<string>>(data);
                if (productCodes != null && productCodes.Any())
                {
                    var removeResult = productCodes.Remove(productCode);
                    if (removeResult)
                    {
                        var userString = JsonSerializer.Serialize(productCodes);
                        await _redisStorage.SetStringAsync(_userEmail, userString);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Something went wrong");
        }
    }
}