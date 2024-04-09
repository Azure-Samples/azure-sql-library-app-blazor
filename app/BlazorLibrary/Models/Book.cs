// Book.cs
namespace BookManagementApp.Models
{
    public class Book
    {
        public int? Id { get; set; } // Make Id nullable
        public string Title { get; set; } = ""; // Initialize with empty string
        public int? Year { get; set; } // Year of publication (nullable)
        public int? Pages { get; set; } // Number of pages (nullable)
        // Add other properties as needed
    }
}
