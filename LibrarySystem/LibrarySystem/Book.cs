using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime Publication_date { get; set; }
        public string ISBN { get; set; }

        /// <summary>
        /// Book constructor
        /// </summary>
        /// <param name="name">Book name</param>
        /// <param name="author">Author name</param>
        /// <param name="category">Categoty of the book</param>
        /// <param name="language">Language of the book</param>
        /// <param name="publication_date">Book publication date</param>
        /// <param name="isbn">International Standard Book Number</param>
        public Book(string name, string author, string category, string language, DateTime publication_date, string isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            Publication_date = publication_date;
            ISBN = isbn;
        }
        public override string ToString()
        {
            return "Name: " + Name + " " + Author + " " + Category + " " + Language + " " + Publication_date + " " + ISBN;
        }
    }
}
