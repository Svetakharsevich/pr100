﻿using Library.Domain.Entities;

namespace Library.Domain.Services
{
    public interface IBooksService
    {
        Task<string> LoadFile(Stream file, string path);
        Task<string> LoadPhoto(Stream file, string path);
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(Book book);
    }
}
