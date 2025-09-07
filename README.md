# GovSite - Government Website Management System

A modern, multi-agency government website management system built with ASP.NET Core 9.0, featuring dynamic page creation, content management, and multi-tenant architecture.

## 🚀 Features

### Core Functionality
- **Multi-Agency Support**: Manage multiple government agencies from a single platform
- **Dynamic Page Builder**: Create custom pages with drag-and-drop content blocks
- **Template System**: Multiple layout templates (Classic, Modern) with customizable themes
- **Content Management**: Rich text editing, image uploads, and media management
- **User Authentication**: Secure admin access with role-based permissions
- **Responsive Design**: Mobile-first design that works on all devices

### Page Builder Features
- **Hero Sections**: Customizable hero banners with images and text
- **Card Blocks**: Create up to 4 cards per row with images, titles, and links
- **Text Blocks**: Rich text content sections
- **Feature Blocks**: Highlight key features and services
- **Gallery Blocks**: Image galleries with responsive grid layout
- **Color Customization**: Custom background and text colors for each section

### Technical Features
- **Entity Framework Core**: MySQL database with migrations
- **ASP.NET Identity**: Secure user management
- **File Upload System**: Secure image uploads with validation
- **CSRF Protection**: Built-in security against cross-site request forgery
- **Arabic Language Support**: Full RTL support and Arabic interface

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 9.0
- **Database**: MySQL with Entity Framework Core
- **Frontend**: Bootstrap 5, jQuery, HTML5, CSS3
- **Authentication**: ASP.NET Identity
- **File Storage**: Local file system with organized uploads
- **Architecture**: MVC Pattern with Repository-like data access

## 📋 Prerequisites

- .NET 9.0 SDK
- MySQL Server 8.0+
- Visual Studio 2022 or VS Code
- Git

## 🚀 Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd GovSite1
```

### 2. Database Setup
1. Create a MySQL database named `GovSitesDb`
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=GovSitesDb;User Id=your_username;Password=your_password;TreatTinyAsBoolean=true;"
  }
}
```

### 3. Install Dependencies
```bash
dotnet restore
```

### 4. Run Database Migrations
```bash
dotnet ef database update
```

### 5. Configure Admin User
Update the admin credentials in `appsettings.json`:
```json
{
  "Seed": {
    "AdminEmail": "admin@yourdomain.com",
    "AdminPassword": "YourSecurePassword123!",
    "AdminPhone": "+1234567890"
  }
}
```

### 6. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` (or the port shown in the console).

## 🏗️ Project Structure

```
GovSite1/
├── Controllers/           # MVC Controllers
│   ├── AccountController.cs      # Authentication
│   ├── AdminController.cs        # Admin dashboard
│   ├── AgenciesController.cs     # Agency management
│   ├── PagesController.cs        # Page management
│   ├── SiteController.cs         # Public site rendering
│   └── UploadController.cs       # File upload handling
├── Data/                 # Data layer
│   ├── AppDbContext.cs           # Entity Framework context
│   └── DbSeeder.cs              # Database seeding
├── Models/               # Data models
│   ├── Agency.cs                # Agency entity
│   ├── Page.cs                  # Page entity
│   ├── PageBlock.cs             # Dynamic content blocks
│   ├── Template.cs              # Layout templates
│   └── AppUser.cs               # User entity
├── Views/                # Razor views
│   ├── Admin/                   # Admin interface
│   ├── Pages/                   # Page management
│   ├── Site/                    # Public site views
│   └── Shared/                  # Shared layouts
├── wwwroot/              # Static files
│   ├── css/                     # Custom styles
│   ├── js/                      # JavaScript files
│   ├── lib/                     # Third-party libraries
│   ├── templates/               # Template styles
│   └── uploads/                 # Uploaded files
└── Migrations/           # Database migrations
```

## 🎯 Usage Guide

### Creating an Agency
1. Login to the admin panel
2. Navigate to "Agencies" → "Create New"
3. Fill in agency details (name, slug, logo)
4. Select a template (Classic or Modern)
5. Save the agency

### Creating Pages
1. Go to "Pages" → "Create New"
2. Select the agency for the page
3. Configure the hero section (title, subtitle, background)
4. Add content blocks:
   - **Cards**: Add up to 4 cards per row with images and links
   - **Text**: Add rich text content
   - **Features**: Highlight key features
   - **Gallery**: Create image galleries
5. Customize colors and styling
6. Save and preview the page

### Managing Content
- **Hero Section**: Set main page title, subtitle, and background image
- **Content Blocks**: Add, edit, reorder, and delete content sections
- **Images**: Upload images with automatic resizing and optimization
- **Colors**: Customize background and text colors for each section
- **Links**: Add external links to cards and buttons

## 🔧 Configuration

### Database Configuration
The application uses Entity Framework Core with MySQL. Key configuration options:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=GovSitesDb;User Id=root;Password=password;TreatTinyAsBoolean=true;"
  }
}
```

### File Upload Configuration
- Maximum file size: 20MB
- Allowed extensions: .png, .jpg, .jpeg, .webp, .gif, .svg
- Upload directory: `wwwroot/uploads/`
- Files are automatically renamed with GUIDs for security

### Authentication Configuration
- Session timeout: 12 hours
- Cookie name: `.GovSite.Auth`
- Login path: `/Account/Login`
- Access denied path: `/Account/AccessDenied`

## 🎨 Customization

### Adding New Templates
1. Create a new layout file in `Views/Shared/Templates/`
2. Add corresponding CSS file in `wwwroot/templates/`
3. Update the `Template` model and database
4. Add template selection in agency creation

### Customizing Styles
- Main site styles: `wwwroot/css/site.css`
- Admin styles: `wwwroot/admin/admin.css`
- Template styles: `wwwroot/templates/`

### Adding New Content Block Types
1. Update the `PageBlock` model
2. Add rendering logic in `Views/Site/DynamicPage.cshtml`
3. Update the JavaScript in create/edit pages
4. Add UI controls for the new block type

## 🔒 Security Features

- **Authentication Required**: All admin functions require login
- **CSRF Protection**: All forms include anti-forgery tokens
- **File Upload Security**: File type validation and size limits
- **SQL Injection Protection**: Entity Framework parameterized queries
- **XSS Protection**: Razor view engine automatic encoding

## 🚀 Deployment

### Production Deployment
1. Update `appsettings.json` with production database connection
2. Set environment to `Production`
3. Run database migrations: `dotnet ef database update`
4. Build the application: `dotnet build --configuration Release`
5. Deploy to your hosting provider

### Docker Deployment (Optional)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GovSite.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GovSite.dll"]
```

## 🐛 Troubleshooting

### Common Issues

**Database Connection Issues**
- Verify MySQL server is running
- Check connection string in `appsettings.json`
- Ensure database exists and user has proper permissions

**File Upload Issues**
- Check `wwwroot/uploads/` directory permissions
- Verify file size limits (20MB max)
- Ensure file extensions are allowed

**Authentication Issues**
- Clear browser cookies
- Check admin credentials in `appsettings.json`
- Verify user exists in database

**Page Not Displaying**
- Check if page is published
- Verify agency slug is correct
- Check for JavaScript errors in browser console

## 📝 API Endpoints

### Public Endpoints
- `GET /{agencySlug}` - Agency home page
- `GET /{agencySlug}/{pageSlug}` - Specific page
- `POST /upload` - File upload (authenticated)

### Admin Endpoints
- `GET /Account/Login` - Login page
- `POST /Account/Login` - Login action
- `GET /admin` - Admin dashboard
- `GET /Pages` - Page management
- `GET /Agencies` - Agency management

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -am 'Add new feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Check the troubleshooting section
- Review the code documentation

## 🔄 Version History

- **v1.0.0** - Initial release with basic CMS functionality
- **v1.1.0** - Added dynamic page builder
- **v1.2.0** - Enhanced upload system and security
- **v1.3.0** - Improved responsive design and card layouts

---

**Built with ❤️ using ASP.NET Core 9.0**
