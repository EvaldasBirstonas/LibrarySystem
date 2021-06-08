using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Adds book to the custom List
        /// </summary>
        /// <param name="book">Book that is being added</param>
        /// <returns>Success boolean value</returns>
        static Boolean AddBook(Book book)
        {
            try
            {
                bookCollection.Add(book);
                updateJson();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Updates the json file by current List
        /// </summary>
        static void updateJson()
        {
            try
            {
                var obj = JsonConvert.SerializeObject(bookCollection, Formatting.Indented);
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
        /// <summary>
        /// Gets all the books from the json file
        /// </summary>
        /// <returns>List of all the books</returns>
        static List<Book> GetAllBooks()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string bookDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Books_json\\";
            string fileName = "books.json";
            return JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(bookDirectory + fileName));
        }
        /// <summary>
        /// Method to take a book and add a customer to it
        /// </summary>
        /// <param name="book">book being taken</param>
        /// <param name="customer">the customer who is taking the book</param>
        /// <returns>Success boolean value</returns>
        static Boolean TakeBook(Book book, Customer customer)
        {
            int index = bookCollection.IndexOf(book);
            if (index == -1)
            {
                return false;
            }
            bookCollection[index] = book;
            bookCollection[index].Customer = customer;
            updateJson();
            return true;
        }
        /// <summary>
        /// Method to return a book
        /// </summary>
        /// <param name="index">index of the book in the List</param>
        /// <returns>Success boolean value</returns>
        static Boolean ReturnBook(int index)
        {
            bookCollection[index].Customer = null;
            updateJson();
            return true;
        }
        static Boolean DeleteBook(Book book)
        {
            return bookCollection.Remove(book);
        }
        static void DeleteBook()
        {
            Console.Clear();
            Console.WriteLine("Provide the books ISBN: ");
            string deleteISBN = Console.ReadLine();
            Book deleteBook;
            try
            {
                int indexOfBook = bookCollection.IndexOf(bookCollection.Where(x => x.ISBN == deleteISBN).Single());
                deleteBook = bookCollection[indexOfBook];
                if (DeleteBook(deleteBook))
                {
                    updateJson();
                    Console.WriteLine("Book deleted successfully");
                }
                else
                {
                    Console.WriteLine("There was an error trying to delete the book");
                }
            }
            catch
            {
                Console.WriteLine("Error fetching the book by ISBN");
                return;
            }
        }
        /// <summary>
        /// Method to display and filter data
        /// </summary>
        static void DisplayData()
        {
            DrawDisplay(bookCollection);
            while (true)
            {
                //drawDisplay(bookCollection);
                string[] displayCommand = Console.ReadLine().Split(" ");
                if (displayCommand.Length == 1 && displayCommand[0] == "exit")
                {
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
                        DrawDisplay(bookCollection.Where(x => x.Author == displayCommand[1]).ToList());
                        break;
                    case "category":
                        DrawDisplay(bookCollection.Where(x => x.Category == displayCommand[1]).ToList());
                        break;
                    case "language":
                        DrawDisplay(bookCollection.Where(x => x.Language == displayCommand[1]).ToList());
                        break;
                    case "isbn":
                        DrawDisplay(bookCollection.Where(x => x.ISBN == displayCommand[1]).ToList());
                        break;
                    case "name":
                        DrawDisplay(bookCollection.Where(x => x.Name == displayCommand[1]).ToList());
                        break;
                    case "availability":
                        if (displayCommand[1] == "taken") 
                            DrawDisplay(bookCollection.Where(x => x.Customer != null).ToList());
                        if (displayCommand[1] == "available")
                            DrawDisplay(bookCollection.Where(x => x.Customer == null).ToList());
                        break;
                    case "reset":
                        DrawDisplay(bookCollection);
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
        static void DrawDisplay(List<Book> bookList)
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
                int indexOfBook = bookCollection.IndexOf(bookCollection.Where(x => x.ISBN == returnISBN).Single());
                returnedBook = bookCollection[indexOfBook];
                if (returnedBook.Customer != null)
                {
                    int late = returnedBook.Customer.ReturnTime.CompareTo(DateTime.Now);
                    if (ReturnBook(indexOfBook))
                    {
                        Console.WriteLine("The book has been successfully returned!");
                        if (late < 0)
                        {
                            Console.WriteLine("But you are late to return your book :'(");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The book was not taken");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Error fetching the book by ISBN");
                return;
            }
        }
        /// <summary>
        /// Method to take a book
        /// </summary>
        static void TakeBook()
        {
            Console.Clear();
            Console.WriteLine("Provide the books ISBN: ");
            string bookISBN = Console.ReadLine();
            Book selectedBook = new Book();
            try
            {
                selectedBook = bookCollection.Where(x => x.ISBN == bookISBN).Single();
            }
            catch
            {
                Console.WriteLine("Error with the books ISBN code in the system");
                return;
            }
            if (selectedBook.Customer != null)
            {
                Console.WriteLine("The book is already taken");
                return;
            }
            Console.WriteLine("Name of taker:");
            string customerName = Console.ReadLine();
            if (bookCollection.Select(x => x.Customer).Where(c => c != null && c.CustomerName == customerName).Count() > 2)
            {
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
                    Console.WriteLine("You can not take the book out for more than 2 months !");
                    return;
                }
                returnTime = DateTime.Now.AddDays(timePeriodInt);
            }
            catch
            {
                Console.WriteLine("Wrong format for days to take out");
                return;
            }
            if (TakeBook(selectedBook, new Customer(customerName, DateTime.Now, returnTime)))
            {
                Console.WriteLine("Book successfully taken!");
            }
            else
            {
                Console.WriteLine("There was an error taking out the book!");
            }
        }
        /// <summary>
        /// Method for adding a book
        /// </summary>
        static void AddBook()
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
                Console.WriteLine("Bad date format, try again");
                return;
            }
            Console.WriteLine("Enter the isbn of the book:");
            string isbn = Console.ReadLine();
            if (AddBook(new Book(name, author, category, language, publication_date, isbn)))
            {
                Console.WriteLine("Book successfully added");
            }
            else
            {
                Console.WriteLine("There was a mistake with adding the book");
            }
        }
    }
}
