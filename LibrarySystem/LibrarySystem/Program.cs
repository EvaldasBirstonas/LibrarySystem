using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LibrarySystem
{
    class Program
    {
        static public List<Book> bookCollection = new List<Book>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            bookCollection = GetAllBooks();
            foreach (Book book in bookCollection)
            {
                Console.WriteLine(book.ToString());
            }
            //Book newBook = new Book("A", "B", "C", "D", DateTime.Now, "TRIETOJ415");
            //Console.WriteLine(newBook.GetISBN());
            //bookCollection.Add(newBook);
            //bookCollection.Add(newBook);
            //AddBook(newBook);
        }

        static Boolean AddBook(Book book)
        {
            try
            {

                var obj = JsonConvert.SerializeObject(bookCollection, Formatting.Indented);
                string workingDirectory = Environment.CurrentDirectory;
                string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
                //Console.WriteLine(System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName);
                string fileName = "books.json";
                Console.WriteLine(fileName);
                File.WriteAllText(bookDirectory + fileName, obj);
                Console.WriteLine(obj.ToString());
                
                /*
                string fileName = book.GetISBN() + ".json";
                string jsonString = JsonSerializer.Serialize(book);
                File.WriteAllText("./Books_json/" + fileName, jsonString);
                Console.WriteLine(jsonString);
                Console.WriteLine(File.ReadAllText(fileName));
                */
                return true;
            }
            catch
            {
                return false;
            }
        }
        static List<Book> GetAllBooks()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
            string fileName = "books.json";
            return JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(bookDirectory + fileName));
        }
    }
}
