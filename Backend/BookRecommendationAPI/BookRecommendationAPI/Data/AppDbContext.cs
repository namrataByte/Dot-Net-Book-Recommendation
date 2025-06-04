using Microsoft.EntityFrameworkCore;
using BookRecommendationAPI.Models;
using System.Collections.Generic;

namespace BookRecommendationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
