using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repository
{
    public class BookDbRepository : IBookstoreRepository<Book>
    {
        BookStoreDbContext _Db;
        public BookDbRepository(BookStoreDbContext Db)
        {
            _Db = Db;
        }
        public void Add(Book entity)
        {
            //entity.Id = books.Max(b => b.Id) + 1;
            _Db.Books.Add(entity);
            _Db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            _Db.Books.Remove(book);
            _Db.SaveChanges();
        }

        public Book Find(int id)
        {
            return _Db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
        }

        public IList<Book> List()
        {
            return _Db.Books.Include(a => a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            _Db.Update(newBook);
            _Db.SaveChanges();
        }
        public List<Book> Search(string term)
        {
            var result = _Db.Books.Include(a => a.Author)
                .Where(b => b.Title.Contains(term)
                || b.Description.Contains(term)
                || b.Author.FullName.Contains(term)).ToList();

            return result;
        }
    }
}