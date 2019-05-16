// using System.Collections.Generic;
// using MySql.Data.MySqlClient;
// using System;
//
// namespace Library.Models
// {
//   public class Librarian
//   {
//     public string Name {get; set;}
//     public int Id {get; set;}
//
//     public Librarian(string name, int id = 0)
//     {
//       Name = name;
//       Id = id;
//     }
//
//     public void Save()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"INSERT INTO librarians (name) VALUES (@name);", conn);
//       cmd.Parameters.AddWithValue("@name", this.Name);
//       cmd.ExecuteNonQuery();
//       Id = (int) cmd.LastInsertedId;
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//
//     public static Librarian Find(int id)
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM librarians WHERE id = (@searchId);", conn);
//       cmd.Parameters.AddWithValue("@searchId", id);
//       var rdr = cmd.ExecuteReader() as MySqlDataReader;
//       int LibrarianId = 0;
//       string librarianName = "";
//
//       while(rdr.Read())
//       {
//         LibrarianId = rdr.GetInt32(0);
//         librarianName = rdr.GetString(1);
//       }
//       Librarian newLibrarian = new Librarian(librarianName, LibrarianId);
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return newLibrarian;
//     }
//
//     public static List<Librarian> GetAll()
//     {
//       List<Librarian> allLibrarians = new List<Librarian> {};
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM librarians;", conn);
//       MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
//       while (rdr.Read())
//       {
//         int librarianId = rdr.GetInt32(0);
//         string librarianName = rdr.GetString(1);
//         Librarian newLibrarian = new Librarian(librarianName, librarianId);
//         allLibrarians.Add(newLibrarian);
//       }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return allLibrarians;
//     }
//
//     public void AddBook(Book newBook)
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"INSERT INTO librarians_books (librarian_id, book_id) VALUES (@LibrarianId, @BookId);";
//       cmd.Parameters.AddWithValue("@LibrarianId", Id);
//       cmd.Parameters.AddWithValue("@BookId", newBook.Id);
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//
//     public List<Book> GetBooks()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"SELECT books.*
//       FROM librarians
//       JOIN librarians_books ON (librarians.id = librarians_books.librarian_id)
//       JOIN books ON (librarians_books.book_id = books.id)
//       WHERE librarians.id = @LibrarianId;", conn);
//       cmd.Parameters.AddWithValue("@LibrarianId", Id);
//       MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
//       List<Book> books = new List<Book> {};
//       while(rdr.Read())
//       {
//         int bookId = rdr.GetInt32(0);
//         string bookTitle = rdr.GetString(1);
//         Book newBook = new Book(bookTitle, bookId);
//         books.Add(newBook);
//       }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return books;
//     }
//
//     public void Delete()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"DELETE FROM librarians WHERE id = @LibrarianId; DELETE FROM librarians_books WHERE librarian_id = @LibrarianId;", conn);
//       cmd.Parameters.AddWithValue("@LibrarianId", this.Id);
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Close();
//       }
//     }
//
//     public static void ClearAll()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"DELETE FROM librarians;", conn);
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//
//     public static void LibrarianBookClearAll()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       MySqlCommand cmd = new MySqlCommand(@"DELETE FROM librarians_books;", conn);
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//
//     public override bool Equals(System.Object otherLibrarian)
//     {
//       if (!(otherLibrarian is Librarian))
//       {
//         return false;
//       }
//       else
//       {
//         Librarian newLibrarian = (Librarian) otherLibrarian;
//         bool idEquality = this.Id.Equals(newLibrarian.Id);
//         bool nameEquality = this.Name.Equals(newLibrarian.Name);
//         return (idEquality && nameEquality);
//       }
//     }
//
//     public override int GetHashCode()
//     {
//       return this.Id.GetHashCode();
//     }
//   }
// }
