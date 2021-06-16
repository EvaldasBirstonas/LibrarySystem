using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace LibrarySystem
{
    public class Program
    {
        private static Library _library = new Library();
        static void Main(string[] args)
        {
            Console.WriteLine("Please use commands: addbook, takebook, returnbook, displaydata, deletebook");
            //Infinite loop for user inputs
            while(true)
            {
                string command = Console.ReadLine();
                switch(command.ToLower())
                {
                    case "addbook":
                        AddBook();
                        break;
                    case "takebook":
                        TakeBook();
                        break;
                    case "returnbook":
                        ReturnBook();
                        break;
                    case "displaydata":
                        DisplayData();
                        break;
                    case "deletebook":
                        DeleteBook();
                        break;
                    default:
                        Console.WriteLine("Unknown command: " + command);
                        break;
                }
            }
        }

        /// <summary>
        /// Method for adding a book
        /// </summary>
        public static void AddBook()
        {
            Console.Clear();
            Console.WriteLine("Enter the name of the book:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the name of the author:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter the name of the category:");
            string category = Console.ReadLine();
            Console.WriteLine("Enter the name of the language:");
            string language = Console.ReadLine();
            Console.WriteLine("Enter the date of publication in format year-month-day:");
            DateTime publication_date;
            try
            {
                publication_date = DateTime.Parse(Console.ReadLine());
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Bad date format, try again");
                return;
            }
            Console.WriteLine("Enter the isbn of the book:");
            string isbn = Console.ReadLine();
            if (_library.AddBook(new Book(name, author, category, language, publication_date, isbn)))
            {
                Console.Clear();
                Console.WriteLine("Book successfully added");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There was a mistake with adding the book");
            }
        }

        /// <summary>
        /// Method to return a book
        /// </summary>
        static void ReturnBook()
        {
            Console.Clear();
            Console.WriteLine("Provide the books ISBN: ");
            string returnISBN = Console.ReadLine();
            Book returnedBook;
            try
            {
                int indexOfBook = _library.GetBookList().IndexOf(_library.GetBookList().Where(x => x.ISBN == returnISBN).Single());
                returnedBook = _library.GetBookList()[indexOfBook];
                if (returnedBook.Customer != null)
                {
                    int late = returnedBook.Customer.ReturnTime.CompareTo(DateTime.Now);
                    if (_library.ReturnBook(indexOfBook))
                    {
                        Console.Clear();
                        Console.WriteLine("The book has been successfully returned!");
                        if (late < 0)
                        {
                            Console.WriteLine("But you are late to return your book :'(");
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("The book was not taken");
                    return;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Error fetching the book by ISBN");
                return;
            }
        }

        /// <summary>
        /// Method to take a book
        /// </summary>
        public static void TakeBook()
        {
            Console.Clear();
            Console.WriteLine("Provide the books ISBN: ");
            string bookISBN = Console.ReadLine();
            Book selectedBook = new Book();
            try
            {
                selectedBook = _library.GetBookList().Where(x => x.ISBN == bookISBN).Single();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Error with the books ISBN code in the system");
                return;
            }
            if (selectedBook.Customer != null)
            {
                Console.Clear();
                Console.WriteLine("The book is already taken");
                return;
            }
            Console.WriteLine("Name of taker:");
            string customerName = Console.ReadLine();
            if (_library.GetBookList().Select(x => x.Customer).Where(c => c != null && c.CustomerName == customerName).Count() > 2)
            {
                Console.Clear();
                Console.WriteLine("The customer has taken out more than 3 books already!");
                return;
            }
            Console.WriteLine("How long is the book being taken in days?");
            string timePeriod = Console.ReadLine();
            DateTime returnTime = new DateTime();
            try
            {
                int timePeriodInt = int.Parse(timePeriod);
                if (timePeriodInt > 60)
                {
                    Console.Clear();
                    Console.WriteLine("You can not take the book out for more than 2 months !");
                    return;
                }
                returnTime = DateTime.Now.AddDays(timePeriodInt);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Wrong format for days to take out");
                return;
            }
            if (_library.TakeBook(selectedBook, new Customer(customerName, DateTime.Now, returnTime)))
            {
                Console.Clear();
                Console.WriteLine("Book successfully taken!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There was an error taking out the book!");
            }
        }

        /// <summary>
        /// Method with steps to delete a book
        /// </summary>
        public static void DeleteBook()
        {
            List<Book> bookList = _library.GetBookList();
            Console.Clear();
            Console.WriteLine("Provide the books ISBN: ");
            string deleteISBN = Console.ReadLine();
            Book deleteBook;
            try
            {
                int indexOfBook = bookList.IndexOf(bookList.Where(x => x.ISBN == deleteISBN).Single());
                deleteBook = bookList[indexOfBook];
                if (_library.DeleteBook(deleteBook))
                {
                    _library.UpdateJson();
                    Console.Clear();
                    Console.WriteLine("Book deleted successfully");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("There was an error trying to delete the book");
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Error fetching the book by ISBN");
                return;
            }
        }

        /// <summary>
        /// Method to display and filter data
        /// </summary>
        public static void DisplayData()
        {
            List<Book> bookList = _library.GetBookList();
            DrawDisplay(bookList);
            while (true)
            {
                //drawDisplay(bookCollection);
                string[] displayCommand = Console.ReadLine().Split(" ");
                if (displayCommand.Length == 1 && displayCommand[0] == "exit")
                {
                    Console.Clear();
                    return;
                }
                if (displayCommand.Length > 2)
                {
                    Console.WriteLine("Too many arguments in command field");
                    continue;
                }
                switch (displayCommand[0].ToLower())
                {
                    case "author":
                        DrawDisplay(bookList.Where(x => x.Author == displayCommand[1]).ToList());
                        break;
                    case "category":
                        DrawDisplay(bookList.Where(x => x.Category == displayCommand[1]).ToList());
                        break;
                    case "language":
                        DrawDisplay(bookList.Where(x => x.Language == displayCommand[1]).ToList());
                        break;
                    case "isbn":
                        DrawDisplay(bookList.Where(x => x.ISBN == displayCommand[1]).ToList());
                        break;
                    case "name":
                        DrawDisplay(bookList.Where(x => x.Name == displayCommand[1]).ToList());
                        break;
                    case "availability":
                        if (displayCommand[1] == "taken") 
                            DrawDisplay(bookList.Where(x => x.Customer != null).ToList());
                        if (displayCommand[1] == "available")
                            DrawDisplay(bookList.Where(x => x.Customer == null).ToList());
                        break;
                    case "reset":
                        DrawDisplay(bookList);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Method to draw the table
        /// </summary>
        /// <param name="bookList">List of contents</param>
        public static void DrawDisplay(List<Book> bookList)
        {
            Console.Clear();
            Console.WriteLine("Type exit to leave or filter by typing:");
            Console.WriteLine("Author authorName");
            Console.WriteLine("Category categoryName");
            Console.WriteLine("Language languageName");
            Console.WriteLine("ISBN isbnCode");
            Console.WriteLine("Name bookName");
            Console.WriteLine("Availability taken/available");
            Console.WriteLine("Or type reset to show all data again");
            Console.WriteLine(String.Format("|{0,15}|{1,15}|{2,15}|{3,15}|{4,20}|{5,15}|{6,15}|", "NAME", "AUTHOR", "CATEGORY", "LANGUAGE", "PUBLICATION DATE", "ISBN", "AVAILABILITY"));
            foreach (Book book in bookList)
            {
                Console.WriteLine(String.Format("|{0,15}|{1,15}|{2,15}|{3,15}|{4,20}|{5,15}|{6,15}|", book.Name, book.Author, book.Category, book.Language, book.Publication_date, book.ISBN, book.Customer == null ? "Available" : "Unavailable"));
            }
        }
    }
}
