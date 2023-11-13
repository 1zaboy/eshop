using Catalog.Models;
using Catalog.Repositories;

namespace Catalog.Services;

public class CatalogService : ICatalogService
{
    private readonly ILogger<CatalogService> _logger;
    private readonly ICatalogRepository _catalogRepository;

    public CatalogService(ICatalogRepository catalogRepository, ILogger<CatalogService> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        return await _catalogRepository.GetBooksAsync();
    }

    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _catalogRepository.GetBookByIdAsync(bookId);
    }

    public async Task AddBookAsync(Book book)
    {
        try
        {
            await _catalogRepository.AddBookAsync(book);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Can not save or update book ({book.Id})");
            throw new Exception("You can not save this book now");
        }
    }

    public async Task UpdateBookAsync(int bookId, Book updatedBook)
    {
        var existingBook = await _catalogRepository.GetBookByIdAsync(bookId);

        if (existingBook != null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Price = updatedBook.Price;

            await _catalogRepository.UpdateBookAsync(existingBook);
        }
        else
        {
            _logger.LogError($"Can not save or update book ({bookId})");
            throw new Exception($"Not found book book (id: {bookId})");
        }
    }

    public async Task DeleteBookAsync(int bookId)
    {
        await _catalogRepository.DeleteBookAsync(bookId);
    }
}