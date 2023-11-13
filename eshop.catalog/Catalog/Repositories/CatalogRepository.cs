using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly CatalogDbContext _context;

    public CatalogRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    } 
}