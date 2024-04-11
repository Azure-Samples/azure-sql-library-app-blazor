// Author.cs
namespace BookManagementApp.Models

{
    public class Author
    {
        public int? id { get; set; } // Make Id nullable

        public string first_name { get; set; } = ""; // Initialize with empty string

        public string? middle_name { get; set; } // Middle name (nullable)

        public string last_name { get; set; } = ""; // Initialize with empty string

        // Add other properties as needed
    }
}