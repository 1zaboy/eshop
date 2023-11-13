namespace Cart.Services;

public interface ICartService
{
    Task<List<string>> GetAll();
    Task Save(string productCode);
    Task Delete(string productCode);
}