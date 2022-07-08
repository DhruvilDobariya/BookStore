using BookStore.Models.DataModels;
using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Repository
{
    public class BookRepository : BaseRepository
    {
        public async Task<ListResponse<Book>> GetBooksAsync(int pageIndex, int pageSize, string keyword)
        {
            List<Book> categories = new List<Book>();
            int totalReocrds;
            if (keyword == null || keyword == "")
            {
                categories = await _context.Books.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                totalReocrds = _context.Books.Count();
            }
            else
            {
                var query = _context.Books.Where(c => keyword.ToLower().Trim() == null || c.Name.ToLower().Contains(keyword.ToLower().Trim())).AsQueryable();
                totalReocrds = query.Count();
                categories = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            }

            return new ListResponse<Book>()
            {
                Records = categories,
                TotalRecords = totalReocrds,
            };
        }

        public Book GetBook(int id)
        {
            return _context.Books.FirstOrDefault(c => c.Id == id);
        }

        public Book AddBook(Book book)
        {
            var entry = _context.Books.Add(book);
            _context.SaveChanges();
            return entry.Entity;
        }

        public Book UpdateBook(Book book)
        {
            var entry = _context.Books.Update(book);
            _context.SaveChanges();
            return entry.Entity;
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(c => c.Id == id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }
    }
}
