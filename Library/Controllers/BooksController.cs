using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;

namespace Library.Controllers
{
  public class BooksController : Controller
  {

    [HttpGet("/authors/{authorId}/books/new")]
    public ActionResult New(int authorId)
    {
       Author author = Author.Find(authorId);
       return View(author);
    }

    [HttpGet("/authors/{authorId}/books/{bookId}")]
    public ActionResult Show(int authorId, int bookId)
    {
      Book book = Book.Find(bookId);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author author = Author.Find(authorId);
      model.Add("book", book);
      model.Add("author", author);
      return View(model);
    }

    [HttpGet("/authors/{authorId}/books/{bookId}/edit")]
    public ActionResult Edit(int authorId, int bookId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author author = Author.Find(authorId);
      model.Add("author", author);
      Book book = Book.Find(bookId);
      model.Add("book", book);
      return View(model);
    }

    [HttpPost("/authors/{authorId}/books/{bookId}")]
    public ActionResult Update(int authorId, int bookId, string newDescription)
    {
      Book book = Book.Find(bookId);
      book.Edit(newDescription);
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
