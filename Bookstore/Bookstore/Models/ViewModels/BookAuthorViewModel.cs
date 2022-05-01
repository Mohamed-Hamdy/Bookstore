using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.ViewModels
{
    public class BookAuthorViewModel
    {
        public int BookId { get; set; }
        public String Title { get; set; }

        public String Description { get; set; }
        public int AuthorId { get; set; }

        public List<Author> Authors { get; set; }
    }
}
