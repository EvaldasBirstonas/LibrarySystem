using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibrarySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Tests
{
    [TestClass()]
    public class BookTests
    {
        [TestMethod()]
        public void BookTest()
        {
            Book book = new Book();
            Assert.IsNotNull(book);
        }

        [TestMethod()]
        public void BookTest1()
        {
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            Book newBook1 = new Book("Name2", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            List<Book> bookList = new List<Book>();
            bookList.Add(newBook);
            bookList.Add(newBook1);
            Assert.AreEqual(newBook1, bookList[1]);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Book newBook = new Book("Name", "Author", "Category", "Language", DateTime.Parse("1999-10-03"), "ISBN817525766-0");
            string expected = "Name: " + newBook.Name + " " + newBook.Author + " " + newBook.Category + " " + newBook.Language + " " + newBook.Publication_date + " " + newBook.ISBN;
            Assert.AreEqual(newBook.ToString(), expected);
        }
    }
}