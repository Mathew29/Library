using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    public string Name {get; set;}
    public int Id {get; set;}

    public Patron(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"INSERT INTO patrons (name) VALUES (@name);", conn);
      cmd.Parameters.AddWithValue("@name", this.Name);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM patrons WHERE id = (@searchId);", conn);
      cmd.Parameters.AddWithValue("@searchId", id);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int PatronId = 0;
      string patronName = "";
      while(rdr.Read())
      {
        PatronId = rdr.GetInt32(0);
        patronName = rdr.GetString(1);
      }
      Patron newPatron = new Patron(patronName, PatronId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newPatron;
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM patrons;", conn);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron newPatron = new Patron(patronName, patronId);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }

    public List<Copy> GetCopies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT copies.* FROM patrons
      JOIN checkouts ON (patrons.id = checkouts.patron_id)
      JOIN copies ON (checkouts.copy_id = copies.id)
      WHERE patrons.id = @PatronId;", conn);
      cmd.Parameters.AddWithValue("@PatronId", Id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Copy> copies = new List<Copy> {};
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        int copyAvailable = rdr GetInt32(2);
        Copy newCopy = new Copy(bookId, copyAvailable, copyId);
        copies.Add(newCopy);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return copies;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM patrons WHERE id = @PatronId; DELETE FROM checkouts WHERE patron_id = @PatronId;", conn);
      cmd.Parameters.AddWithValue("@PatronId", this.Id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void PatronCopyClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM checkouts;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = this.Id.Equals(newPatron.Id);
        bool nameEquality = this.Name.Equals(newPatron.Name);
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }
  }
}
