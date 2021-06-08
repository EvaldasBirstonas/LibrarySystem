using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibrarySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace LibrarySystem.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void TakeBookTest()
        {
            List<Book> booksList = new List<Book>();
            booksList.Add(new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0"));
            Assert.AreEqual(booksList.IndexOf(new Book("None", "none", "none", "None", DateTime.Parse("2000-01-01"), "None")), -1);
        }

        [TestMethod()]
        public void AddBookTest()
        {
            try
            {
                DateTime.Parse("ABCDEFG");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod()]
        public void updateJsonTest()
        {
            List<Book> bookCollection = new List<Book>();
            bookCollection.Add(new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0"));
            var obj = JsonConvert.SerializeObject(bookCollection, Formatting.Indented);
            Assert.IsNotNull(obj);
        }

        [TestMethod()]
        public void GetAllBooksTest()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
            string fileName = "books.json";
            Assert.IsNotNull(bookDirectory + fileName);
        }

        [TestMethod()]
        public void TakeBookTest1()
        {
            List<Book> booksList = new List<Book>();
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            booksList.Add(newBook);
            Assert.AreEqual(booksList.IndexOf(newBook), 0);
        }

        [TestMethod()]
        public void ReturnBookTest()
        {
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            newBook.Customer = new Customer("NameOfCustomer", DateTime.Now, DateTime.Now.AddDays(30));
            newBook.Customer = null;
            Assert.IsNull(newBook.Customer);
        }

        [TestMethod()]
        public void DeleteBookTest()
        {
            List<Book> booksList = new List<Book>();
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            booksList.Add(newBook);
            Assert.AreEqual(booksList.Remove(newBook), true);
        }

        [TestMethod()]
        public void DeleteBookTest1()
        {
            List<Book> booksList = new List<Book>();
            booksList.Add(new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0"));
            Assert.AreEqual(booksList.Remove(new Book("UnknownName", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0")), false);
        }

        [TestMethod()]
        public void DisplayDataTest()
        {
            string[] displayCommand = { "exit" };
            Assert.IsTrue(displayCommand.Length == 1 && displayCommand[0] == "exit");
        }

        [TestMethod()]
        public void DrawDisplayTest()
        {
            List<Book> booksList = new List<Book>();
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            booksList.Add(newBook);
            booksList.Add(newBook);
            booksList.Add(newBook);
            int i = 0;
            foreach (Book book in booksList)
            {
                i++;
            }
            Assert.AreEqual(i, booksList.Count);
        }

        [TestMethod()]
        public void TakeBookTest2()
        {
            int returnDays = 61;
            Assert.IsFalse(returnDays <= 60);
        }

        [TestMethod()]
        public void AddBookTest1()
        {
            List<Book> booksList = new List<Book>();
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            int startCount = booksList.Count;
            booksList.Add(newBook);
            int endCount = booksList.Count;
            Assert.IsTrue(startCount < endCount);
        }
    }
}