using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Library
    {
        private static List<Book> _bookCollection = GetAllBooks();

        public Library()
        {

        }

        /// <summary>
        /// Gets all the books from the json file
        /// </summary>
        /// <returns>List of all the books</returns>
        public static List<Book> GetAllBooks()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
            string fileName = "books.json";
            return JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(bookDirectory + fileName));
        }

        public List<Book> GetBookList()
        {
            return _bookCollection;
        }

        /// <summary>
        /// Adds book to the custom List
        /// </summary>
        /// <param name="book">Book that is being added</param>
        /// <returns>Success boolean value</returns>
        public Boolean AddBook(Book book)
        {
            try
            {
                _bookCollection.Add(book);
                UpdateJson();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to take a book and add a customer to it
        /// </summary>
        /// <param name="book">book being taken</param>
        /// <param name="customer">the customer who is taking the book</param>
        /// <returns>Success boolean value</returns>
        public Boolean TakeBook(Book book, Customer customer)
        {
            int index = _bookCollection.IndexOf(book);
            if (index == -1)
            {
                return false;
            }
            _bookCollection[index] = book;
            _bookCollection[index].Customer = customer;
            UpdateJson();
            return true;
        }

        /// <summary>
        /// Method to return a book
        /// </summary>
        /// <param name="index">index of the book in the List</param>
        /// <returns>Success boolean value</returns>
        public Boolean ReturnBook(int index)
        {
            _bookCollection[index].Customer = null;
            UpdateJson();
            return true;
        }

        /// <summary>
        /// Method that tries to delete a book from a list
        /// </summary>
        /// <param name="book">Book that needs to be deleted</param>
        /// <returns>Boolean of success</returns>
        public Boolean DeleteBook(Book book)
        {
            return _bookCollection.Remove(book);
        }

        /// <summary>
        /// Updates the json file by current List
        /// </summary>
        public void UpdateJson()
        {
            try
            {
                var obj = JsonConvert.SerializeObject(_bookCollection, Formatting.Indented);
                string workingDirectory = Environment.CurrentDirectory;
                string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
                string fileName = "books.json";
                File.WriteAllText(bookDirectory + fileName, obj);
            }
            catch
            {
                Console.WriteLine("Error while updating the json file");
            }
        }
    }
}
