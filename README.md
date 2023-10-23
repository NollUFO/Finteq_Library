# Finteq Library
Basic Console application that performs crud operations on books within an xml file.

# 1. Description
This app was created to showcase my C# skills during the interview process with Finteq. Finteq Library can create, update, delete and view books within its associated xml file. 

I have used xml as my persistable data structure because of how easily and (somewhat)simply integrates with C#. I also have a previous experience with xml and feel most comfortable with it.

# 2. How Finteq Library works
Finteq Library communicates with a 'Library.xml' file stored in the bin/net6.0 folder to persist data that has been modified by the user through a console interface. A user can add a book, view all books, update a book, delete a book, check in a book, return a book and finally search for a book based on title or author.

On startup Finteq Library will pull all the books stored in the Library.xml file into a list of books. All CRUD operations on then performed through this list of books until the user wishes to exit. On Exit Finteq Library uses a linq query to reformat the list of books back into xml and adjusts the xml to fit the list of books.

# 3. Credits
I would like to thank the Finteq Development team for giving me the oppurtunity to partake and enjoy this challenge - It's been a blast!

# 4. References
Chatgpt has been used slightly in regards to the linq queries as I had not interacted with Linq to xml environments much before.
