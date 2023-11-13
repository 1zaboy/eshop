using Catalog.Models;

namespace Catalog.Repositories;

public interface ICatalogRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book> GetBookByIdAsync(int bookId);
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int bookId);
}