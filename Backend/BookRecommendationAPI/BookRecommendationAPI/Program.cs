using System;
using Microsoft.EntityFrameworkCore;
using BookRecommendationAPI.Services;
using BookRecommendationAPI.Data;
using Microsoft.OpenApi.Models; // For OpenApiString
using Swashbuckle.AspNetCore.SwaggerGen; // For SwaggerGenOptions

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")  // your frontend URL

                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger with schema filter for readable enums
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumSchemaFilter>(); // Register custom schema filter for enums
});

// Register the GoogleBooksService with HttpClient
builder.Services.AddHttpClient<GoogleBooksService>();

// Configure database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors("AllowReactApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
