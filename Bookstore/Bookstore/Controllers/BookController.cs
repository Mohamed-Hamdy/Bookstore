using Bookstore.Models;
using Bookstore.Models.Repository;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book> _bookRepository;
        private readonly IBookstoreRepository<Author> _authorRepository;

        public BookController(IBookstoreRepository<Book> bookRepository , IBookstoreRepository<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
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
                Authors = _authorRepository.List().ToList()
            };
            return View(model);
        }

        // POST: BookController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            try
            {
                var author = _authorRepository.Find(model.AuthorId);
                Book book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = author
                };
                _bookRepository.Add(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
                Authors = _authorRepository.List().ToList()
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
                var author = _authorRepository.Find(viewmodel.AuthorId);
                Book book = new Book
                {
                    Title = viewmodel.Title,
                    Description = viewmodel.Description,
                    Author = author
                };

                _bookRepository.Update(viewmodel.BookId , book);
                return RedirectToAction(nameof(Index));
            }
            catch
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
    }
}
