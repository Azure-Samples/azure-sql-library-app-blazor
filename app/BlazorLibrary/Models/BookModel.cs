// Book.cs
namespace BookManagementApp.Models
{
    public class BookModel
    {
        public int? Id { get; set; } // Make Id nullable
        public string Title { get; set; } = ""; // Initialize with empty string
        public string Year { get; set; } // Year of publication (nullable)
        public string Pages { get; set; } // Number of pages (nullable)
        // Add other properties as needed
    }
}
