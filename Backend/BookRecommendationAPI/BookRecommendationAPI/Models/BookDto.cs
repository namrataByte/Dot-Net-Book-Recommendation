namespace BookRecommendationAPI.Models
{
    public class BookDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string InfoLink { get; set; }
    }

}
