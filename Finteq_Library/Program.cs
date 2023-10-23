using System;

public class Program
{
    static void Main()
    {
        BookList booklist = new BookList();

        while (true)
        {
            switch (MainMenu())
            {
                case 1:
                    AddNewBook(booklist);
                    break;
                case 2:
                    ViewAllBooks(booklist);
                    break;
                case 3:
                    UpdateBook(booklist);
                    break;
                case 4:
                    DeleteBook(booklist);
                    break;
                case 5:
                    CheckOut(booklist);
                    break;
                case 6:
                    ReturnBook(booklist);
                    break;
                case 7:
                    FindBook(booklist);
                    break;
                case 8:
                    booklist.SaveListToXml();
                    return;
            }
        }
    }

    static int MainMenu()
    {
            Console.Clear();
            Console.WriteLine("Welcome to Finteq Library Management System!");
            Console.WriteLine();
            Console.WriteLine("1. Add a new book.");
            Console.WriteLine("2. View all books.");
            Console.WriteLine("3. Update a book's details.");
            Console.WriteLine("4. Delete a book.");
            Console.WriteLine("5. Check out a book.");
            Console.WriteLine("6. Return a book.");
            Console.WriteLine("7. Search for a book.");
            Console.WriteLine("8. Exit.");

        // Check user input
        while (true)
        {
            Console.Write("Enter your choice: ");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int choice) && (choice >= 1 && choice <= 8)) 
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }


    static void AddNewBook(BookList booklist)
    {
        showHeader("Add a book");
        booklist.AddNewBook();
        enterToContinue();
    }

    static void ViewAllBooks(BookList booklist)
    {
        showHeader("View all books");
        booklist.ViewAllBooks();
        enterToContinue();
    }

    static void UpdateBook(BookList booklist)
    {
        showHeader("Update a book");
        booklist.UpdateBook(GetBookId("update"));
        enterToContinue();
    }

    static void DeleteBook(BookList booklist)
    {
        showHeader("Delete a book");
        booklist.DeleteBook(GetBookId("delete"));
        enterToContinue();
    }

    static void CheckOut(BookList booklist)
    {
        showHeader("Check out a book");
        booklist.CheckOut(GetBookId("check out"));
        enterToContinue();
    }

    static void ReturnBook(BookList booklist)
    {
        showHeader("Return a book");
        booklist.ReturnBook(GetBookId("return"));
        enterToContinue();
    }

    static void FindBook(BookList booklist)
    {
        showHeader("Find a book");
        booklist.FindBook();
        enterToContinue();
    }

    static string GetBookId( string change)
    {
        Console.Write("Please enter the book Id of the book you want to {0}: ", change);
        return Console.ReadLine() ?? string.Empty;
    }
    
    static void showHeader(string function)
    {
        Console.Clear();
        Console.WriteLine("Finteq Library - {0}", function);
        Console.WriteLine();
    }

    static void enterToContinue()
    {
        Console.WriteLine();
        Console.Write("Press enter to continue...");
        while (Console.ReadKey().Key != ConsoleKey.Enter) { }
    }
}