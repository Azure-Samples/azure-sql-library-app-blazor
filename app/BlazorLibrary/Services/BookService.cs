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
    
        public class BookService
        {
            private readonly HttpClient _httpClient;

            public BookService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task AddBookAsync(Book book)
            {

                var bookWithoutId = new
                {
                    book.Title,
                    book.Year,
                    book.Pages
                };

                var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:5001/api/Book", bookWithoutId);
                response.EnsureSuccessStatusCode(); // Throw on error status code
            }

            public async Task<bool> UpdateBookAsync(Book book)
            {

                var bookWithoutId = new
                {
                    book.Title,
                    book.Year,
                    book.Pages
                };

                var response = await _httpClient.PatchAsJsonAsync($"http://127.0.0.1:5001/api/Author/id/{book.Id}", bookWithoutId);
                return response.IsSuccessStatusCode;
            }

            public async Task DeleteBookAsync(int id)
            {
                var response = await _httpClient.DeleteAsync($"http://127.0.0.1:5001/api/Book/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error status code
            }
    }
}
