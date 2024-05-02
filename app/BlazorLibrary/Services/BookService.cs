// BookService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BookManagementApp.Models;
using BookManagementApp.Extensions;

namespace BookManagementApp.Services
{
    public class BookResponse
    {
        public List<Book> Value { get; set; }
    }

    public class BookAuthorIdResponse
    {
        public List<BookAuthorIdModel> Value { get; set; }
    }

    public class BookService
    {
        private readonly HttpClient _httpClient;
        private string ApiUrl = "";

        public BookService(HttpClient httpClient, IConfiguration Configuration)
        {
            _httpClient = httpClient;
            ApiUrl = Configuration.GetValue<string>("ApiUrl");
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            try
            {
                var responseStream = await _httpClient.GetStreamAsync($"{ApiUrl}/api/Book/id/{id}");
                var reader = new StreamReader(responseStream);
                var responseString = await reader.ReadToEndAsync();

                var bookResponse = JsonSerializer.Deserialize<BookResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var book = bookResponse.Value.FirstOrDefault();

                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching author: {ex}");
                return null;
            }
        }

        public async Task<BookResponse> AddBookAsync(Book book)
        {

            var bookWithoutId = new
            {
                title = book.Title,
                year = book.Year,
                pages = book.Pages
            };

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/api/Book", bookWithoutId);
            response.EnsureSuccessStatusCode(); // Throw on error status code

            return await response.Content.ReadFromJsonAsync<BookResponse>();
        }

        public async Task<List<Author>?> GetBookAuthorsAsync(string bookId)
        {

            var responseAuthors = await _httpClient.GetFromJsonAsync<BookAuthorIdResponse>($"{ApiUrl}/api/BookAuthor?%24filter=book_id%20eq%20{bookId}");

            if (responseAuthors != null){
                var lstAuthorsIds = responseAuthors.Value.Select(x => "id eq " + x.author_id).ToList();

                var authorsIds = string.Join(" or ", lstAuthorsIds);

                if (lstAuthorsIds.Count() > 0){
                    var authors = await _httpClient.GetFromJsonAsync<AuthorResponse>($"{ApiUrl}/api/Author?%24filter={authorsIds}");
                    return authors.Value.ToList();
                }
            }

            return new List<Author>();
        }

        public async Task AddAuthorBookAsync(int? BookId, int? AuthorId)
        {

            var bookWithoutId = new
            {
                author_id = AuthorId,
                book_id = BookId
            };

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/api/BookAuthor", bookWithoutId);
            response.EnsureSuccessStatusCode(); // Throw on error status code
        }

        public async Task DeleteAuthorBookAsync(int? BookId, int? AuthorId)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/api/BookAuthor/author_id/{AuthorId}/book_id/{BookId}");
            response.EnsureSuccessStatusCode(); // Throw on error status code
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {

            var bookWithoutId = new
            {
                title = book.Title,
                year = book.Year,
                pages = book.Pages
            };

            var response = await _httpClient.PatchAsJsonAsync($"{ApiUrl}/api/Book/id/{book.Id}", bookWithoutId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/api/Book/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
