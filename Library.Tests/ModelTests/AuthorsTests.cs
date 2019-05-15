using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTest : IDisposable
  {

    public void Dispose()
    {
      Author.ClearAll();
      Book.ClearAll();
      Author.AuthorBookClearAll();
    }
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void AuthorConstructor_CreatesInstanceOfAuthor_Author()
    {
      Author newAuthor = new Author("test author");
      Assert.AreEqual(typeof(Author), newAuthor.GetType());
    }

    [TestMethod]
    public void GetAll_ReturnsAllAuthorObjects_AuthorList()
    {
      //Arrange
      string name01 = "Work";
      string name02 = "School";
      Author newAuthor1 = new Author(name01);
      newAuthor1.Save();
      Author newAuthor2 = new Author(name02);
      newAuthor2.Save();
      List<Author> newList = new List<Author> { newAuthor1, newAuthor2 };

      //Act
      List<Author> result = Author.GetAll();
      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectAuthor_Author()
    {
      //Arrange
      string name01 = "Work";
      Author newAuthor1 = new Author(name01);
      newAuthor1.Save();
      //Act
      Author result = Author.Find(newAuthor1.Id);

      //Assert
      Assert.AreEqual(newAuthor1, result);
    }

    [TestMethod]
    public void GetBooks_ReturnsAllAuthorBooks_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Household chores");
      testAuthor.Save();
      Book testBook1 = new Book("Mow the lawn");
      testBook1.Save();
      Book testBook2 = new Book("Buy plane ticket");
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook1);
      testAuthor.AddBook(testBook2);
      List<Book> savedBooks = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook1, testBook2};

      //Assert
      CollectionAssert.AreEqual(testList, savedBooks);
    }

    [TestMethod]
    public void Test_AddBook_AddsBookToAuthor()
    {
      //Arrange
      Author testAuthor = new Author("Household chores");
      testAuthor.Save();
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      Book testBook2 = new Book("Water the garden");
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.AddBook(testBook2);
      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book>{testBook, testBook2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesAuthorAssociationsFromDatabase_AuthorList()
    {
      //Arrange
      Book testBook = new Book("Mow the lawn");
      testBook.Save();
      string testName = "Home stuff";
      Author testAuthor = new Author(testName);
      testAuthor.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.Delete();
      List<Author> resultBookAuthors = testBook.GetAuthors();
      List<Author> testBookAuthors = new List<Author> {};

      //Assert
      CollectionAssert.AreEqual(testBookAuthors, resultBookAuthors);
    }

  }
}
