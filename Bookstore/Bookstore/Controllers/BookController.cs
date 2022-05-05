using Bookstore.Models;
using Bookstore.Models.Repository;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book> _bookRepository;
        private readonly IBookstoreRepository<Author> _authorRepository;
        private readonly IWebHostEnvironment _hosting;

        public BookController(IBookstoreRepository<Book> bookRepository , IBookstoreRepository<Author> authorRepository , IWebHostEnvironment hosting)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _hosting = hosting;
        }
        // GET: BookController1
        public ActionResult Index()
        {
            var book = _bookRepository.List();
            return View(book);
        }

        // GET: BookController1/Details/5
        public ActionResult Details(int id)
        {
            var book = _bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController1/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filename = UploadFiles(model.File) ?? string.Empty;
                try
                {
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author from the list!";
                        
                        return View(GetAllAuthors());
                    }
                    var author = _authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImagesUrl = filename
                    };
                    _bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "You have to fill all required inputs!");
            return View(GetAllAuthors());
           
        }

        // GET: BookController1/Edit/5
        public ActionResult Edit(int id)
        {
            var book = _bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id; 
            var viewmodel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = _authorRepository.List().ToList(),
                ImageUrl = book.ImagesUrl
            };

            return View(viewmodel);
        }

        // POST: BookController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel viewmodel)
        {
            try
            {
                string filename = UploadFiles(viewmodel.File , viewmodel.ImageUrl);
                
                var author = _authorRepository.Find(viewmodel.AuthorId);
                Book book = new Book
                {
                    Id = viewmodel.BookId,
                    Title = viewmodel.Title,
                    Description = viewmodel.Description,
                    Author = author,
                    ImagesUrl = filename
                };

                _bookRepository.Update(viewmodel.BookId , book);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: BookController1/Delete/5
        public ActionResult Delete(int id)  
        {
            var book = _bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Book book)
        {
            try
            {
                _bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = _authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please select an author ---" });
            return authors;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };

            return vmodel;
        }
        string UploadFiles(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(_hosting.WebRootPath, "Uploads");
                string fullpath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullpath, FileMode.Create));
                return file.FileName;
            }
            return null;
        }
        string UploadFiles(IFormFile file , string imageurl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(_hosting.WebRootPath, "Uploads");
                string newpath = Path.Combine(uploads, file.FileName);

                // Delete old file
                string oldpath = Path.Combine(uploads, imageurl);
                if (oldpath != newpath)
                {
                    System.IO.File.Delete(oldpath);
                    // save new file 
                    file.CopyTo(new FileStream(newpath, FileMode.Create));

                }
                return file.FileName;
            }
            return imageurl;

        }

        public ActionResult Search(string term)
        {
            var result = _bookRepository.Search(term);
            return View("Index", result);

        }
    }
}
