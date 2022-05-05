using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repository
{
    public class AuthorDbRepository : IBookstoreRepository<Author>
    {
        BookStoreDbContext _Db;
        public AuthorDbRepository(BookStoreDbContext Db)
        {
            _Db = Db;
        }

        public void Add(Author entity)
        {
            //entity.Id = Authors.Max(b => b.Id) + 1;
            _Db.Authors.Add(entity);
            _Db.SaveChanges();

        }

        public void Delete(int id)
        {
            var author = Find(id);
            _Db.Authors.Remove(author);
            _Db.SaveChanges();

        }

        public Author Find(int id)
        {
            var author = _Db.Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return _Db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            return _Db.Authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            _Db.Update(newAuthor);
            _Db.SaveChanges();
        }
    }
}
