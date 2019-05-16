using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class BooksController : Controller
  {
    [HttpGet("/books")]
    public ActionResult Index()
    {
      List<Book> allBooks = Book.GetAll();
      return View(allBooks);
    }

    [HttpGet("/books/new")]
    public ActionResult New(int authorId)
    {
       Author author = Author.Find(authorId);
       return View(author);
    }

    [HttpGet("/books/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(id);
      List<Author> bookAuthors = selectedBook.GetAuthors();
      List<Author> allAuthors = Author.GetAll();
      model.Add("book", selectedBook);
      model.Add("bookAuthors", bookAuthors);
      model.Add("allAuthors", allAuthors);
      return View(model);
    }

    [HttpPost("/books/{bookId}/authors/new")]
    public ActionResult AddAuthor(int bookId, int authorName)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(id);
      Author newAuther = new Author(authorName);
      newAuthor.Save();
      selectedBook.GetAuthors();
      List<Author> bookAuthors = selectedBook.GetAuthors();
      model.Add("book", selectedBook);
      model.Add("bookAuthors", bookAuthors);
      return View("Show", bookAuthors);
      // 
      //
      // Book foundBook = Book.Find(bookId);
      // Author foundAuthor = Author.Find(authorId);
      // foundBook.AddAuthor(foundAuthor);
      // return RedirectToAction("Show", model);
    }


    [HttpPost("/books")]
    public ActionResult Create(string bookTitle)
    {
      Book newBook = new Book(bookTitle);
      newBook.Save();
      List<Book> allBooks = Book.GetAll();
      return View("Index", allBooks);
    }

    [HttpGet("/books/{bookId}")]
    public ActionResult Show(int authorId, int bookId)
    {
      Book book = Book.Find(bookId);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author author = Author.Find(authorId);
      model.Add("book", book);
      model.Add("author", author);
      return View(model);
    }

    [HttpGet("/books/{bookId}/edit")]
    public ActionResult Edit(int authorId, int bookId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author author = Author.Find(authorId);
      model.Add("author", author);
      Book book = Book.Find(bookId);
      model.Add("book", book);
      return View(model);
    }

    [HttpPost("/books/{bookId}")]
    public ActionResult Update(int authorId, int bookId, string newTitle)
    {
      Book book = Book.Find(bookId);
      book.Edit(newTitle);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author author = Author.Find(authorId);
      model.Add("author", author);
      model.Add("book", book);
      return View("Show", model);
    }

    [HttpPost("/books/delete")]
    public ActionResult DeleteAll()
    {
      Book.ClearAll();
      return View();
    }


  }
}
