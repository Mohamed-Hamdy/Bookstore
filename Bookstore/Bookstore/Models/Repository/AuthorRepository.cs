using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repository
{
    public class AuthorRepository : IBookstoreRepository<Author>
    {
        List<Author> Authors;
        public AuthorRepository()
        {
            Authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FullName = "Author 1"                
                },
                new Author
                {
                    Id = 2,
                    FullName = "Author 2"
                },
                new Author
                {
                    Id = 3,
                    FullName = "Author 3"
                },
            };
        }

        public void Add(Author entity)
        {
            Authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            Authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return Authors;
        }

        public void Update(int id, Author newAuthor)
        {
            var author = Find(id);
            author.FullName = newAuthor.FullName;
        }
    }
}
