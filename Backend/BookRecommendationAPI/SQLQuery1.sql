CREATE TABLE UserPreferences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Genre NVARCHAR(100),
    FavoriteAuthor NVARCHAR(100),
    Language NVARCHAR(50),
    UserNote NVARCHAR(MAX)
);