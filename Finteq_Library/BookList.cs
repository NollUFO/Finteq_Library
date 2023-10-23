using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class BookList
{
    public List<Book> booklist { get; set; }
    private string path = Directory.GetCurrentDirectory();

    public BookList()
    {
        booklist = new List<Book>();
        if (!File.Exists(path + "\\Library.xml"))
        {
            CreateXml();
        }
        else
        {
            MoveXmlToList();
        }
    }

    public void CreateXml()
    {
        XDocument Library = new XDocument(new XElement("Library"));
        Library.Save("Library.xml");
    }

    private void MoveXmlToList()
    {
        XDocument library = XDocument.Load("Library.xml");
        if (library.Root != null)
        {
            IEnumerable<XElement> xmlBooks = library.Root.Elements("Book"); // Assuming your XML has "Book" elements.

            foreach (XElement xmlBook in xmlBooks)
            {
                Book book = new Book
                {
                    BookId = xmlBook.Attribute("Id")?.Value,
                    Title = xmlBook.Element("Title")?.Value,
                    Author = xmlBook.Element("Author")?.Value,
                    PublicationYear = int.TryParse(xmlBook.Element("PublicationYear")?.Value, out int year) ? year : -1,
                    IsCheckedOut = bool.TryParse(xmlBook.Element("IsCheckedOut")?.Value, out bool isCheckedOut) ? isCheckedOut : false,
                };
                booklist.Add(book);
            }
        }
    }

    public void SaveListToXml()
    {
        XDocument library = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("Library",
                booklist.Select(book => new XElement("Book",
                    new XAttribute("Id", book.BookId ?? "Unknown Id"),
                    new XElement("Title", book.Title),
                    new XElement("Author", book.Author),
                    new XElement("PublicationYear", book.PublicationYear),
                    new XElement("IsCheckedOut", book.IsCheckedOut))
                )
            )
        );

        // Save the new XML document, overwriting the previous data
        library.Save(Path.Combine(path, "Library.xml"));
    }

    public void AddNewBook()
    {
        Book book = new Book();
        while (true)            // Check if Id does not exist
        {
            Console.Write("Book ID: ");
            string? checkId = Console.ReadLine();
            Book? checkbook = findBookById(checkId ?? string.Empty); 
            if (checkbook != null)
            {
                Console.WriteLine("Book already exists");
            }
            else
            {
                book.BookId = checkId;
                break;
            }
        }
        Console.Write("Title: ");
        book.Title = Console.ReadLine();
        Console.Write("Author: ");
        book.Author = Console.ReadLine();
        int pubYear = -1;
        while (true)            // Ensure user enters valid year
        {
            Console.Write("PublicationYear: ");
            string strPubYear = Console.ReadLine() ?? string.Empty;
            pubYear = checkPubYear(strPubYear);
            if (pubYear != -1)
            {
                break;
            }
        }

        booklist.Add(book);
        Console.WriteLine("You have added '{0}'!", book.Title);
    }

    public void ViewAllBooks()
    {
        foreach (var book in booklist)
        {
            DisplayBook(book);
        }
    }

    public void UpdateBook(string bookId)
    {
        Book? book = findBookById(bookId);
        // Book Exists
        if (book != null)
        {
            Console.WriteLine("Please leave the entry unchanged if you want to keep its current value.");
            Console.WriteLine();

            // Start of changing book values
            Console.Write("Current Title: {0}. New Title: ", book.Title);
            string Title = Console.ReadLine() ?? string.Empty;
            Console.Write("Current Author: {0}. New Author: ", book.Author);
            string Author = Console.ReadLine() ?? string.Empty;

            //Check if Publication year is to be changed

            int pubYear = -1;
            while (true)
            {
                Console.Write("Current Publication Year: {0}. New Publication Year: ", book.PublicationYear);
                string strPubYear = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(strPubYear))
                {
                    break;
                }
                pubYear = checkPubYear(strPubYear);
                if (pubYear != -1)
                {
                    break;
                }

            }

            // Confirm User wants to update values
            bool confirm = ConfirmUserChanges("update", book);
            if (confirm)
            {
                if (!string.IsNullOrWhiteSpace(Title)) { book.Title = Title; }
                if (!string.IsNullOrWhiteSpace(Author)) { book.Author = Author; }
                if (pubYear != -1) { book.PublicationYear = pubYear; }
                Console.WriteLine("You have updated '{0}'!", book.Title);
            }
            else
            {
                DiscardChanges();
            }
        }
        else
        {
            Console.WriteLine("A book with Id '{0}' does not exist.", bookId);
        }
    }

    public void DeleteBook(string bookId)
    {
        Book? book = findBookById(bookId);
        if (book != null)
        {
            bool Confirm = ConfirmUserChanges("delete", book);
            if (Confirm)
            {
                booklist.Remove(book);
                Console.WriteLine("You have deleted '{0}'!", book.Title);
            }
            else
            {
                DiscardChanges();
            }
        }
        else
        {
            Console.WriteLine("A book with Id '{0}' does not exist.", bookId);
        }
    }

    public void CheckOut(string bookId)
    {
        Book? book = findBookById(bookId);
        if (book != null)
        {
            if (book.IsCheckedOut == false)
            {
                // Book can be checked out
                // Confirm user wants to check out book
                bool Confirm = ConfirmUserChanges("check out", book);
                if (Confirm)
                {
                    book.IsCheckedOut = true;
                    Console.WriteLine("You have checked out '{0}'!", book.Title);
                }
                else
                {
                    DiscardChanges();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("'{0}' has already been checked out.", book.Title);
            }
        }
        else
        {
            Console.WriteLine("A book with Id '{0}' does not exist.", bookId);
        }
    }

    public void ReturnBook(string bookId)
    {
        Book? book = findBookById(bookId);
        if (book != null)
        {
            if (book.IsCheckedOut == true)
            {
                // Book can be checked out
                // Confirm user wants to check out book
                bool Confirm = ConfirmUserChanges("return", book);
                if (Confirm)
                {
                    book.IsCheckedOut = false;
                    Console.WriteLine("You have returned '{0}'!", book.Title);
                }
                else
                {
                    DiscardChanges();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("'{0}' has already been returned.", book.Title);
            }
        }
        else
        {
            Console.WriteLine("A book with Id '{0}' does not exist.", bookId);
        }
    }

    public void FindBook()
    {
        Console.WriteLine("Would you like to search by Title(1) or Author(2)?");
        Console.Write("Please choose one of the options above: ");
        if (int.TryParse(Console.ReadLine(), out int searchFactor) && (searchFactor == 1 || searchFactor == 2))
        {
            Console.Write("What would you like to search for: ");
            string? searchValue = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchValue.ToLower();
            }
            Console.WriteLine();

            var books = (searchFactor == 1) //Check against title
                ? booklist.Where(book => book.Title?.ToLower() == searchValue)
                : (searchFactor == 2) //Check against author
                    ? booklist.Where(book => book.Author?.ToLower() == searchValue)
                    : Enumerable.Empty<Book>(); // Make books empty if no values found

            if (books.Any())
            {
                foreach (var book in books)
                {
                    DisplayBook(book);
                }
            }
            else
            {
                Console.WriteLine("No books have been found against the given criteria.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 1(Title) or 2(Author).");
        }
    }

    private int checkPubYear(string pubYear)
    {
        try
        {
            int currYear = DateTime.Now.Year + 1;
            if (int.TryParse(pubYear, out int validYear))
            {
                if (validYear > 0 && validYear <= currYear)
                {
                    return validYear;
                }
                else
                {
                    Console.WriteLine("Please enter a year between 0 and {0}.", currYear);
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Please enter a year between 0 and {0}.", currYear);
                return -1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        return -1;
    } 

    private Book? findBookById( string bookId)
    {
        foreach (var book in booklist)
        {
            // Book has been found
            if (book.BookId == bookId)
            {
                return book;
            }
        }
        
        // Book does not exist
        return null; 
    }

    private static void DisplayBook(Book book)
    {
        Console.WriteLine("Id: {0}", book.BookId);
        Console.WriteLine("Title: {0}", book.Title);
        Console.WriteLine("Author: {0}", book.Author);
        Console.WriteLine("Publication Year: {0}", book.PublicationYear);
        Console.WriteLine("Is Checked Out: {0}", book.IsCheckedOut);
        Console.WriteLine();
    }

    private static bool ConfirmUserChanges(string confirmation, Book book)
    {
        Console.Write("Are you sure you want to {0} '{1}'? (y/n) ", confirmation, book.Title);
        string choice = Console.ReadLine() ?? string.Empty;
        while (true)
        {
            if (choice.ToLower() == "y")
            {
                return true;
            }
            else if (choice.ToLower() == "n")
            {
                return false;
            }
            else
            {
                Console.Write("Please enter 'y' or 'n' to confirm you choice. ");
            }
        }
    }

    private static void DiscardChanges()
    {
        Console.WriteLine("Changes Discarded.");
        Console.WriteLine("Returning to main menu...");
        System.Threading.Thread.Sleep(1000);
    }
}
