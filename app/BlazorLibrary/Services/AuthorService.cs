//AuthorService.cs

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

    public class AuthorResponse
    {
        public List<Author> Value { get; set; }
    }

    public class AuthorService
    {
        private readonly HttpClient _httpClient;
        private string ApiUrl = "";

        public AuthorService(HttpClient httpClient, IConfiguration Configuration)
        {
            _httpClient = httpClient;
            ApiUrl = Configuration.GetValue<string>("ApiUrl");
        }

        public async Task<List<Author>?> GetAuthors()
        {
            try
            {
                var responseStream = await _httpClient.GetStreamAsync($"{ApiUrl}/api/Author");
                var reader = new StreamReader(responseStream);
                var responseString = await reader.ReadToEndAsync();

                var authorResponse = JsonSerializer.Deserialize<AuthorResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var authors = authorResponse.Value.ToList();
                return authors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching author: {ex}");
                return null;
            }
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            try
            {
                var responseStream = await _httpClient.GetStreamAsync($"{ApiUrl}/api/Author/id/{id}");
                var reader = new StreamReader(responseStream);
                var responseString = await reader.ReadToEndAsync();

                var authorResponse = JsonSerializer.Deserialize<AuthorResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var author = authorResponse.Value.FirstOrDefault();
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching author: {ex}");
                return null;
            }
        }

        public async Task AddAuthorAsync(Author author)
        {
            var authorWithoutId = new
            {
                author.first_name,
                author.middle_name,
                author.last_name
            };

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/api/Author", authorWithoutId);
            response.EnsureSuccessStatusCode(); 
        }
        
        public async Task<bool> UpdateAuthorAsync(Author author)
        {

            var authorWithoutId = new
            {
                author.first_name,
                author.middle_name,
                author.last_name
            };

            var response = await _httpClient.PatchAsJsonAsync($"{ApiUrl}/api/Author/id/{author.id}", authorWithoutId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/api/Author/id/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}        