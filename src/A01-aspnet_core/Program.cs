using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. Konfiguracja Dependency Injection (DI)
// ============================================

// Rejestracja DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=.;Database=ProjectManagerDb;Trusted_Connection=true;TrustServerCertificate=true;"));

// Rejestracja usług biznesowych
builder.Services.AddScoped<IProjectService, ProjectService>();

// ============================================
// 2. Dodanie kontrolerów i widoków (MVC)
// ============================================
builder.Services.AddControllersWithViews();

// Rejestracja Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// ============================================
// 3. Konfiguracja Request Pipeline (middleware)
// ============================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ============================================
// 4. Autentykacja i Autoryzacja
// ============================================
app.UseAuthentication();
app.UseAuthorization();

// ============================================
// 5. Mapowanie tras
// ============================================

// MVC Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}");

// Blazor Routes
app.MapBlazorHub();
app.MapRazorPages();

// ============================================
// 6. Inicjalizacja bazy danych
// ============================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Tworzenie bazy danych i migracji
    try
    {
        dbContext.Database.EnsureCreated();
        Console.WriteLine("✅ Baza danych została zainicjalizowana pomyślnie!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Błąd podczas inicjalizacji bazy danych: {ex.Message}");
    }
}

// ============================================
// 7. Uruchomienie aplikacji
// ============================================
Console.WriteLine("🚀 Aplikacja ProjectManager uruchamia się...");
app.Run();

/**
 * ARCHITEKTURA APLIKACJI
 * 
 * 1. MODELS (ProjectManager.Models)
 *    - Project: Główna encja projektu
 *    - Task: Zadania powiązane z projektami (1:N)
 *    - ProjectStatus, TaskStatus: Enumeracje
 *    - ProjectAnalytics: ViewModel dla analiz
 * 
 * 2. DATA ACCESS (ProjectManager.Data)
 *    - ApplicationDbContext: DbContext EF Core
 *    - Relacje 1:N z cascade delete
 *    - Seed data dla przykładowych danych
 * 
 * 3. SERVICES (ProjectManager.Services)
 *    - IProjectService: Interfejs
 *    - ProjectService: Implementacja z LINQ
 *    - CRUD operacje
 *    - Analityka i filtrowanie
 * 
 * 4. CONTROLLERS (ProjectManager.Controllers)
 *    - ProjectsController: HTTP endpoints
 *    - GET Index: Lista z wyszukiwaniem i sortowaniem
 *    - GET Create/POST Create: Tworzenie
 *    - GET Details: Szczegóły z analitiką
 *    - GET Edit/POST Edit: Edycja
 *    - POST Delete: Usuwanie
 * 
 * 5. VIEWS (Razor)
 *    - Projects/Index.cshtml: Lista projektów
 *    - Projects/Details.cshtml: Szczegóły z analitiką
 *    - Projects/Create.cshtml: Formularz tworzenia
 *    - Projects/Edit.cshtml: Formularz edycji
 *    - Projects/Analytics.cshtml: Dashboard analityk
 * 
 * 6. BLAZOR COMPONENTS
 *    - ProjectDashboard.razor: Interaktywny dashboard
 *    - TaskEditor.razor: Edytor zadań w real-time
 * 
 * ŚCIEŻKA ŻĄDANIA:
 * 
 * HTTP Request
 *     ↓
 * Controllers (ProjectsController)
 *     ↓
 * Services (ProjectService) - LINQ queries
 *     ↓
 * DbContext (ApplicationDbContext)
 *     ↓
 * Database (SQL Server)
 *     ↓
 * Views (Razor templates)
 *     ↓
 * HTTP Response (HTML/JSON)
 */
