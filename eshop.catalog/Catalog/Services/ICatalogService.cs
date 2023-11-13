using Catalog.Models;

namespace Catalog.Services;

public interface ICatalogService
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book> GetBookByIdAsync(int bookId);
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(int bookId, Book updatedBook);
    Task DeleteBookAsync(int bookId);
}