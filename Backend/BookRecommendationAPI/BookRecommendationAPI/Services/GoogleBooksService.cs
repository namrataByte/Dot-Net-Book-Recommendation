using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BookRecommendationAPI.Models;

namespace BookRecommendationAPI.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Book>> GetBooksAsync(string query, int? maxResults, string orderBy = null)
        {
            if (maxResults.HasValue && maxResults.Value > 40)
            {
                maxResults = 40; // Google Books API limit
            }

            var url = $"https://www.googleapis.com/books/v1/volumes?q={query}";

            if (maxResults.HasValue)
            {
                url += $"&maxResults={maxResults.Value}";
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                url += $"&orderBy={orderBy}";
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            var results = new List<Book>();

            if (!document.RootElement.TryGetProperty("items", out var items))
                return results;

            foreach (var item in items.EnumerateArray())
            {
                var volumeInfo = item.GetProperty("volumeInfo");

                string title = volumeInfo.GetProperty("title").GetString();

                List<string> authors = volumeInfo.TryGetProperty("authors", out var authorArray)
                    ? authorArray.EnumerateArray().Select(a => a.GetString()).ToList()
                    : new List<string>();

                string description = volumeInfo.TryGetProperty("description", out var descElement)
                    ? descElement.GetString()
                    : "No description available.";

                string thumbnail = null;
                if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks))
                {
                    if (imageLinks.TryGetProperty("smallThumbnail", out var smallThumb))
                        thumbnail = smallThumb.GetString();
                    else if (imageLinks.TryGetProperty("thumbnail", out var thumb))
                        thumbnail = thumb.GetString();
                }

                if (!string.IsNullOrEmpty(thumbnail) && thumbnail.StartsWith("http://"))
                {
                    thumbnail = "https://" + thumbnail.Substring("http://".Length);
                }

                if (string.IsNullOrEmpty(thumbnail))
                {
                    thumbnail = "https://via.placeholder.com/128x195?text=No+Image";
                }

                string infoLink = null;
                if (volumeInfo.TryGetProperty("infoLink", out var linkElement))
                {
                    infoLink = linkElement.GetString();
                }
                else if (volumeInfo.TryGetProperty("previewLink", out var previewElement))
                {
                    infoLink = previewElement.GetString();
                }

                results.Add(new Book
                {
                    Title = title,
                    Authors = authors,
                    Description = description,
                    Thumbnail = thumbnail,
                    InfoLink = infoLink
                });
            }

            return results;
        }

        // Improved trending books method with better query and ordering
        public async Task<List<Book>> GetTrendingBooksAsync(int maxResults = 10)
        {
            // Using a subject filter and ordering by newest releases to simulate trending books
            string trendingQuery = "subject:fiction";
            string orderBy = "newest";
            return await GetBooksAsync(trendingQuery, maxResults, orderBy);
        }
    }
}
