namespace BookRecommendationAPI.Models
{
    public class BookSearchRequest
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int? MaxResults { get; set; }
    }
}
