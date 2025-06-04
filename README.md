# Dot-Net Book Recommendation

A simple book recommendation web application built with React (frontend) and .NET Core (backend).  
This app allows users to search for book recommendations by querying the **Google Books API**, filtering by title, author, genre, and language, and displaying detailed book information including covers, descriptions, and links.

---

## Features

- Search books by Title, Author, Genre, and Language  
- Select maximum number of results  
- Fetches real-time book data from **Google Books API**  
- View detailed book information in a modal popup  
- Responsive and user-friendly UI  

---

## Technologies Used

- **Frontend:** React, React Router, CSS  
- **Backend:** ASP.NET Core Web API (.NET 7)  
- **API:** Google Books API for fetching book data  
- **Version Control:** Git, GitHub  

---

## Configuration

- API calls are made to **Google Books API** endpoints.  
- Make sure your API keys (if any) are stored securely and excluded from version control (e.g., in `appsettings.json` or environment variables).  
- Update the frontend/backend configuration if you use any API keys or change request URLs.

---

## Getting Started

### Prerequisites

- Node.js and npm installed  
- .NET SDK installed (version 7 or above)  
- Git installed  

### Running Locally

1. Clone the repository  
   ```bash
   git clone https://github.com/namrataByte/Dot-Net-Book-Recommendation.git
   cd Dot-Net-Book-Recommendation
