using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repository
{
    public class BookRepository : IBookstoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Description = "Description 1 ",
                    Title = "java Programming",
                    Author = new Author()
                },
                new Book
                {
                    Id = 2,
                    Description = "Description 2 ",
                    Title = "Python Programming",
                    Author = new Author()

                },new Book
                {
                    Id = 3,
                    Description = "Description 3 ",
                    Title = "C# Programming",
                    Author = new Author()
                },
            };
        }
        public void Add(Book entity)
        {
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            return books.SingleOrDefault(b => b.Id == id);
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update(int id , Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;

        }

       
    }
}
