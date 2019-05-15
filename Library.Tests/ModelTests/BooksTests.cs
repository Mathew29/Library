using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Tests
{
  [TestClass]
  public class BookTest : IDisposable
  {
    public void Dispose()
    {
      Book.ClearAll();
      Author.ClearAll();
      Book.BookAuthorClearAll();
    }

    public BookTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void BookConstructor_CreatesInstanceOfBook_Book()
    {
      Book newBook = new Book("test");
      Assert.AreEqual(typeof(Book), newBook.GetType());
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyListFromDatabase_BookList()
    {
      //Arrange
      List<Book> newList = new List<Book> { };

      //Act
      List<Book> result = Book.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfTitleAreTheSame_Book()
    {
      Book firstBook = new Book("Mow the lawn");
      Book secondBook = new Book("Mow the lawn");
      Assert.AreEqual(firstBook, secondBook);
    }

    [TestMethod]
    public void Save_SavesToDatabase_BookList()
    {
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsBooks_BookList()
    {
      //Arrange
      string title01 = "Walk the dog";
      string title02 = "Wash the dishes";
      Book newBook1 = new Book(title01);
      newBook1.Save();
      Book newBook2 = new Book(title02);
      newBook2.Save();
      List<Book> newList = new List<Book> { newBook1, newBook2 };

      //Act
      List<Book> result = Book.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Save_AssignsIdObject_Id()
    {
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      Book savedBook = Book.GetAll()[0];
      int result = savedBook.Id;
      int testId = testBook.Id;
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectBookFromDatabase_Book()
    {
      //Arrange
      Book testBook = new Book("Mow the lawn");
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.Id);

      //Assert
      Assert.AreEqual(testBook, foundBook);
    }

    [TestMethod]
    public void GetAuthors_ReturnAllBooksAuthors_AuthorList()
    {
      //Arrange
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      Author testAuthor1 = new Author("Home Stuff");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Work Stuff");
      testAuthor2.Save();

      //Act
      testBook.AddAuthor(testAuthor1);
      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author> {testAuthor1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void AddAuthor_AddsAuthorToBook_AuthorList()
    {
      //Arrange
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      Author testAuthor = new Author("Home stuff");
      testAuthor.Save();

      //Act
      testBook.AddAuthor(testAuthor);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesBookAssociationsFromDatabase_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Home stuff");
      testAuthor.Save();
      string testTitle = "Mow the lawn";
      Book testBook = new Book(testTitle);
      testBook.Save();

      //Act
      testBook.AddAuthor(testAuthor);
      testBook.Delete();
      List<Book> resultAuthorBooks = testAuthor.GetBooks();
      List<Book> testAuthorBooks = new List<Book> {};

      //Assert
      CollectionAssert.AreEqual(testAuthorBooks, resultAuthorBooks);
    }

    // [TestMethod]
    // public void CompletedBook_ChangesIsCompletePropertyToTrue_Bool()
    // {
    //
    //   Author testAuthor = new Author("Home stuff");
    //   testAuthor.Save();
    //   string testTitle = "Mow the lawn";
    //   Book testBook = new Book(testTitle);
    //   testBook.Save();
    //   testAuthor.AddBook(testBook);
    //
    //   testBook.AddAuthor(testAuthor);
    //
    //   testBook.CompletedBook();
    //   List<Book> resultAuthorBooks = testAuthor.GetBooks();
    //
    //   Assert.AreEqual(true, testBook.GetIsComplete());
    // }
  }
}
