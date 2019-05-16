using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {

    [HttpGet("/authors")]
    public ActionResult Index()
    {
      List<Author> allAuthors = Author.GetAll();
      return View(allAuthors);
    }

    [HttpGet("/authors/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/authors")]
    public ActionResult Create(string authorTitle)
    {
      Author newAuthor = new Author(authorTitle);
      newAuthor.Save();
      List<Author> allAuthors = Author.GetAll();
      return View("Index", allAuthors);
    }

    [HttpGet("/authors/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author selectedAuthor = Author.Find(id);
      List<Book> authorBooks = selectedAuthor.GetBooks();
      model.Add("author", selectedAuthor);
      model.Add("books", authorBooks);
      return View(model);
    }

    // // This one creates new Books within a given Author, not new Authors:
    [HttpPost("/authors/{authorId}/books")]
    public ActionResult Create(int authorId, string bookTitle, DateTime bookDueDate)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author foundAuthor = Author.Find(authorId);
      Book newBook = new Book(bookTitle);
      newBook.Save();
      // foundAuthor.AddBook(newBook);
      List<Book> authorBooks = foundAuthor.GetBooks();
      model.Add("books", authorBooks);
      model.Add("author", newBook);
      return View("Show", model);
    }

  }
}
