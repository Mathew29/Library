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

  }
}
