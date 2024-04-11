using Library.Domain.Entities;

namespace Library.ViewModels
{
    public class BooksCatalogViewModel
    {
        public List<Book> Books { get; set; }
        public List<Category> Categories { get; set; }
    }
}
