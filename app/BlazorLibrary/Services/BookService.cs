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

        public async Task AddBookAsync(Book book)
        {

            var bookWithoutId = new
            {
                title = book.Title,
                year = book.Year,
                pages = book.Pages
            };

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/api/Book", bookWithoutId);
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
