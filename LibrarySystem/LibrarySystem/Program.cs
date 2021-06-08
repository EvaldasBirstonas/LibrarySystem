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
            while(true)
            {
                string command = Console.ReadLine();
                switch(command.ToLower())
                {
                    case "addbook":
                        Console.WriteLine("Enter the name of the book:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter the name of the author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("Enter the name of the category:");
                        string category = Console.ReadLine();
                        Console.WriteLine("Enter the name of the language:");
                        string language = Console.ReadLine();
                        Console.WriteLine("Enter the date of publication in format year-month-day:");
                        DateTime publication_date = new DateTime();
                        try
                        {
                            publication_date = DateTime.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Bad date format, try again");
                            break;
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
                        break;
                    case "takebook":
                        Console.WriteLine("Provide the books ISBN: ");
                        string bookISBN = Console.ReadLine();
                        Book selectedBook;
                        try
                        {
                            selectedBook = bookCollection.Where(x => x.ISBN == bookISBN).Single();
                        }
                        catch
                        {
                            Console.WriteLine("Error with the books ISBN code in the system");
                            break;
                        }
                        if (selectedBook.Customer != null)
                        {
                            Console.WriteLine("The book is already taken");
                            break;
                        }
                        Console.WriteLine("Name of taker:");
                        string customerName = Console.ReadLine();
                        if (bookCollection.Select(x => x.Customer).Where(c => c != null && c.CustomerName == customerName).Count() > 2)
                        {
                            Console.WriteLine("The customer has taken out more than 3 books already!");
                            break;
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
                                break;
                            }
                            returnTime = DateTime.Now.AddDays(timePeriodInt);
                        }
                        catch
                        {
                            Console.WriteLine("Wrong format for days to take out");
                            break;
                        }
                        if (TakeBook(selectedBook, new Customer(customerName, DateTime.Now, returnTime)))
                        {
                            Console.WriteLine("Book successfully taken!");
                        }
                        else
                        {
                            Console.WriteLine("There was an error taking out the book!");
                        }
                        break;
                    case "returnbook":
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
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error fetching the book by ISBN");
                        }
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
    }
}
