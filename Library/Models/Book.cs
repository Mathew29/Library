using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Book
  {
    public string Title {get; set;}
    public int Id {get; set;}

    public Book (string title, int id = 0)
    {
      Title = title;
      Id = id;
    }
    // _isComplete = isComplete;
    // public void CompletedBook()
    // {
    //   _isComplete = !(this.GetIsComplete());
    // }

    public void Edit(string newTitle)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"UPDATE books SET title = @newTitle WHERE id = @searchId;", conn);
      cmd.Parameters.AddWithValue("@searchId", Id);
      cmd.Parameters.AddWithValue("@newTitle", newTitle);
      cmd.ExecuteNonQuery();
      Title = newTitle;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"INSERT INTO books (title) VALUES (@BookTitle);", conn);
      cmd.Parameters.AddWithValue("@BookTitle", this.Title);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM books;", conn);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, bookId);
        allBooks.Add(newBook);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBooks;
    }


    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM books WHERE id = (@thisId);", conn);
      cmd.Parameters.AddWithValue("@thisId", id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bookId = 0;
      string bookTitle = "";
      while(rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookTitle = rdr.GetString(1);
      }
      Book foundBook = new Book(bookTitle, bookId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundBook;
    }

    public List<Author> GetAuthors()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT author_id FROM authors_books WHERE book_id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@bookId", Id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> authorIds = new List<int> {};
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        authorIds.Add(authorId);
      }
      rdr.Dispose();
      List<Author> authors = new List<Author> {};
      foreach (int authorId in authorIds)
      {
        MySqlCommand command = new MySqlCommand(@"SELECT * FROM authors WHERE id = @AuthorId;", conn);
        command.Parameters.AddWithValue("@AuthorId", authorId);
        MySqlDataReader commandRdr = command.ExecuteReader() as MySqlDataReader;
        while(commandRdr.Read())
        {
          int thisAuthorId = commandRdr.GetInt32(0);
          string authorName = commandRdr.GetString(1);
          Author foundAuthor = new Author(authorName, thisAuthorId);
          authors.Add(foundAuthor);
        }
        commandRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return authors;
    }


    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);
      cmd.Parameters.AddWithValue("@AuthorId", newAuthor.Id);
      cmd.Parameters.AddWithValue("@BookId", Id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM books WHERE id = @BookId; DELETE FROM authors_books WHERE book_id = @BookId;", conn);
      cmd.Parameters.AddWithValue("@BookId", this.Id);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void BookAuthorClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM authors_books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.Id == newBook.Id);
        bool titleEquality = (this.Title == newBook.Title);
        // bool isCompleteEquality = (this.GetIsComplete() == newBook.GetIsComplete());
        return (idEquality && titleEquality);
      }
    }

  }
}
