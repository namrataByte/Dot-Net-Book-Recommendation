using System.Net.Http;
using System.Text.Json;
using BookRecommendationAPI.Models;
using BookRecommendationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookRecommendationAPI.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly GoogleBooksService _booksService;

        public RecommendationsController(GoogleBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpPost("books")]
        public async Task<IActionResult> GetRecommendations([FromBody] BookSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title) &&
                string.IsNullOrWhiteSpace(request.Genre) &&
                string.IsNullOrWhiteSpace(request.Author))
            {
                return BadRequest("At least one of 'title', 'genre', or 'author' must be provided.");
            }

            Language validLanguage = Language.en;
            if (!string.IsNullOrWhiteSpace(request.Language) &&
                !Enum.TryParse<Language>(request.Language, true, out validLanguage))
            {
                return BadRequest("Invalid language code. Use ISO 639-1 codes like 'en', 'fr', etc.");
            }

            var queryParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.Title)) queryParts.Add(request.Title);
            if (!string.IsNullOrWhiteSpace(request.Genre)) queryParts.Add(request.Genre);
            if (!string.IsNullOrWhiteSpace(request.Author)) queryParts.Add($"inauthor:{request.Author}");

            string query = string.Join("+", queryParts) + $"&langRestrict={validLanguage.ToString().ToLower()}";

            var books = await _booksService.GetBooksAsync(query, request.MaxResults);

            if (books == null || books.Count == 0)
                return NotFound("No books found matching your criteria.");

            return Ok(books);
        }

        // ✅ Updated Trending Endpoint using GoogleBooksService
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingBooks()
        {
            // Better query - popular fiction books ordered by newest
            var trendingBooks = await _booksService.GetBooksAsync("subject:fiction", 10, orderBy: "newest");

            if (trendingBooks == null || trendingBooks.Count == 0)
                return NotFound("No trending books found.");

            return Ok(trendingBooks);
        }

    }
}



















/* [HttpGet("books")]
         public async Task<IActionResult> GetRecommendations([FromQuery] string genre, [FromQuery] string author)
         {
             var query = $"{genre}+inauthor:{author}";
             var result = await _booksService.GetBooksAsync(query);

             if (result == null || !result.Any())
                 return NotFound("No recommendations found.");

             return Ok(result);
         }*/
