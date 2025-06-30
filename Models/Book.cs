namespace HotelApi.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string LastName { get; set; }
    public List<Book> Titles { get; set; } = new List<Book>();
}
public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
}