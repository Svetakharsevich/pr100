using Library.Domain.Entities;

namespace Library.Domain.Services
{
    public interface IBooksReader
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<List<Book>> FindBooksAsync(string searchString, int categoryId);
        Task<Book?> FindBookAsync(int bookId);
        Task<List<Category>> GetCategoriesAsync();
    }
}
