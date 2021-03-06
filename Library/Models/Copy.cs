using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Copy
  {
    public int BookId {get; set;}
    public int Available {get; set;}
    public int Id {get; set;}

    public Copy (int bookId, int available, int id = 0)
    {
      BookId = bookId;
      Available = available;
      Id = id;
    }

    // Adds copies to the copy table
    public void AddCopy(Copy newCopy)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT books.title
      FROM copies
      JOIN books ON (copies.book_id = books.id));", conn);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    // This creates a list all books for the titles - access to books table from the copies controller
    // public List<Book> GetBooks()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   MySqlCommand cmd = new MySqlCommand(@"SELECT book_id FROM copies WHERE copy_id = @copyId", conn);
    //   cmd.Parameters.AddWithValue("@copyId", Id);
    //   MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   List<int> bookIds = new List<int> {};
    //   while(rdr.Read())
    //   {
    //     int bookId = rdr.GetInt32(2);
    //     bookIds.Add(bookId);
    //   }
    //   rdr.Dispose();
    //   List<Book> books = new List<Book> {};
    //   foreach(int bookId in bookIds)
    //   {
    //     MySqlCommand bookQuery = new MySqlCommand(@"SELECT * FROM books WHERE id = @BookId;", conn);
    //     bookQuery.Parameters.AddWithValue("@BookId", bookId);
    //     MySqlDataReader bookQueryRdr = bookQuery.ExecuteReader() as MySqlDataReader;
    //     while(bookQueryRdr.Read())
    //     {
    //       int thisBookId = bookQueryRdr.GetInt32(0);
    //       string bookTitle = bookQueryRdr.GetString(1);
    //       Book foundBook = new Book(bookTitle, thisBookId);
    //       books.Add(foundBook);
    //     }
    //     bookQueryRdr.Dispose();
    //   }
    //   conn.Close();
    //   if(conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return books;
    // }


    // Need tested but should be working

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"INSERT INTO copies (book_id, available) VALUES (@CopyBookId, @CopyAvailable);", conn);
      cmd.Parameters.AddWithValue("@CopyBookId", this.BookId);
      cmd.Parameters.AddWithValue("@CopyAvailable", this.Available);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
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
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM copies WHERE id = @CopyId; DELETE FROM checkouts WHERE copy_id = @CopyId;", conn);
      cmd.Parameters.AddWithValue("@CopyId", this.Id);
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
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = (this.Id == newCopy.Id);
        bool availableEquality = (this.Available == newCopy.Available);
        bool bookIdEquality = (this.BookId == newCopy.BookId);
        return (idEquality && availableEquality && bookIdEquality);
      }
    }

  }
}
