# 📊 DIAGRAMS - A01: ASP.NET Core Architecture

## Diagram 1: MVC Request Flow

Jak żądanie HTTP przechodzi przez aplikację:

```mermaid
graph LR
    Browser["🌐 Browser"]
    HTTP["HTTP Request<br/>GET /projects"]
    
    subgraph ASPCore["🔷 ASP.NET Core"]
        Middleware["⚙️ Middleware Pipeline"]
        Routing["🗺️ Routing"]
        Controller["🎮 Controller"]
    end
    
    subgraph Business["💼 Business Logic"]
        Service["🔧 Service (LINQ)"]
        DbContext["💾 DbContext"]
    end
    
    Database["🗄️ SQL Server"]
    Response["HTML Response"]
    
    Browser -->|1. HTTP| HTTP
    HTTP -->|2. Process| Middleware
    Middleware -->|3. Route| Routing
    Routing -->|4. Dispatch| Controller
    Controller -->|5. Query| Service
    Service -->|6. ORM| DbContext
    DbContext -->|7. SQL| Database
    
    Database -->|8. Data| DbContext
    DbContext -->|9. Objects| Service
    Service -->|10. Business Logic| Controller
    Controller -->|11. Model| Response
    Response -->|12. HTML| Browser
```

---

## Diagram 2: Database Schema - Project 1:N Task

Relacja między tabelami:

```mermaid
erDiagram
    PROJECT ||--o{ TASK : has
    
    PROJECT {
        int Id PK "Primary Key"
        string Name
        string Description
        datetime StartDate
        datetime EndDate
        int ProgressPercentage
        string Status
        bool IsFavorite
    }
    
    TASK {
        int Id PK "Primary Key"
        string Title
        string Description
        int Priority
        string Status
        datetime DueDate
        int ProjectId FK "Foreign Key"
    }
```

**Relacja:**
- 1 Project → Wiele Tasks (1:N)
- Usunięcie Project → Usuwa wszystkie Tasks (Cascade Delete)
- Indeksy: ProjectId, Status columns

---

## Diagram 3: Dependency Injection Container

Jak DI kontener rejestruje i injektuje usługi:

```mermaid
graph TB
    Register["📋 Rejestracja Usług"]
    
    Register -->|builder.Services.AddDbContext| DbContext["ApplicationDbContext"]
    Register -->|builder.Services.AddScoped| Service["IProjectService<br/>ProjectService"]
    Register -->|builder.Services.AddControllers| Controller["ProjectsController"]
    
    Request["🌐 HTTP Request"]
    Request -->|Dependency Injection| Controller
    
    Controller -->|Injected| Service
    Service -->|Injected| DbContext
    DbContext -->|Managed By| SQLServer["SQL Server"]
    
    style Register fill:#ffd700
    style Service fill:#90EE90
    style DbContext fill:#87CEEB
```

**Lifecycle:**
- **Singleton**: Jedna instancja dla całej aplikacji
- **Scoped**: Nowa instancja per HTTP request (default)
- **Transient**: Nowa instancja za każdym razem

---

## Diagram 4: Entity Framework Core Data Access Pattern

```mermaid
graph TB
    LINQ["📝 LINQ Query"]
    
    LINQ -->|await .ToListAsync| Translation["🔄 Translation to SQL"]
    
    Translation -->|DbContext| DbSet["DbSet<Project>"]
    
    DbSet -->|EF Core| SQL["
    SELECT * FROM Projects<br/>
    WHERE Status = 'Active'<br/>
    ORDER BY StartDate DESC
    "]
    
    SQL -->|Execute| Database["🗄️ Database"]
    
    Database -->|Results| Materialize["🔨 Materialize to Objects"]
    
    Materialize -->|Return| Objects["List<Project>"]
    
    Objects -->|Use in| Controller["🎮 Controller"]
    
    style LINQ fill:#FFB6C1
    style SQL fill:#87CEEB
    style Objects fill:#90EE90
```

---

## Diagram 5: LINQ Query Execution Pipeline

```mermaid
graph LR
    Query["IQueryable<Project>"]
    
    Query -->|.Where| Filter["Filter: Status == Active"]
    Filter -->|.OrderBy| Sort["Sort: By StartDate DESC"]
    Sort -->|.Include| Load["Eager Load: Tasks"]
    Load -->|.ToListAsync| Execute["🚀 Execute SQL"]
    
    Execute -->|Database Engine| Result["📦 Results"]
    
    Result -->|Lazy Evaluation| Delay1{Execute Now?}
    Delay1 -->|.ToListAsync| Delay2["✅ Execute"]
    Delay1 -->|No Call| Delay3["⏸️ Deferred"]
    
    style Query fill:#FFE4E1
    style Execute fill:#FFB6C1
    style Result fill:#90EE90
```

---

## Diagram 6: Razor Rendering Pipeline

Jak Razor konwertuje .cshtml na HTML:

```mermaid
graph TB
    CSHTML["📄 Index.cshtml<br/>@model List<Project>"]
    
    CSHTML -->|Parse| Parser["🔍 Parser"]
    
    Parser -->|Separate| Markup["HTML Markup"]
    Parser -->|Separate| CSharp["C# Code"]
    
    Markup -->|As-is| HtmlGen["📝 HTML Generation"]
    CSharp -->|Compile| Compile["⚙️ Compile C#"]
    
    HtmlGen -->|Merge| Merge["🔗 Merge C# Output"]
    Compile -->|Output| Merge
    
    Merge -->|Create| Dynamic["Dynamic Class"]
    
    Dynamic -->|Execute with Model| Execute["🚀 Execute with @Model"]
    
    Execute -->|forEach task| Loop["Loop: @foreach"]
    Loop -->|Generate| Rows["<tr> for each task"]
    
    Rows -->|Final| HTML["📤 HTML Output"]
    
    HTML -->|Send| Browser["🌐 Browser"]
    
    style CSHTML fill:#FFE4E1
    style HTML fill:#90EE90
```

---

## Diagram 7: Blazor Component Lifecycle & Communication

```mermaid
graph TB
    Browser["🌐 Browser"]
    
    Browser -->|1. Initial Load| Component["🎨 Blazor Component"]
    
    Component -->|2. OnInitialized| Init["Initialize State"]
    
    Init -->|3. LoadData| Service["Inject IProjectService"]
    
    Service -->|4. Call Async| Database["💾 Load from DB"]
    
    Database -->|5. Return Data| Service
    
    Service -->|6. Set Property| Component
    
    Component -->|7. StateChanged| Render["⚡ Render HTML"]
    
    Render -->|8. Send via WebSocket| Browser
    
    Browser -->|9. User Click| Event["Event Handler"]
    
    Event -->|10. @onclick| Handler["Event Handler Method"]
    
    Handler -->|11. Update State| Component
    
    Component -->|12. Re-render| Render
    
    Render -->|13. Send Delta| Browser
    
    style Browser fill:#E6F3FF
    style Component fill:#FFE4E1
    style Service fill:#90EE90
    style Database fill:#87CEEB
```

---

## Diagram 8: Authorization & Authentication Flow

```mermaid
graph LR
    Request["🌐 Request"]
    
    Request -->|HttpContext| Auth["🔐 Authentication"]
    
    Auth -->|Claims-based| Identity["User Identity"]
    
    Identity -->|Principal| Authz["🛡️ Authorization"]
    
    Authz -->|Policy| Policy["[Authorize]<br/>[AllowAnonymous]"]
    
    Policy -->|Success| Controller["✅ Execute Controller"]
    
    Policy -->|Fail| Forbidden["❌ 403 Forbidden"]
    
    Controller -->|Return| Response["Response"]
    
    Forbidden -->|Redirect| Login["Login Page"]
    
    style Auth fill:#FFB6C1
    style Authz fill:#FFD700
    style Controller fill:#90EE90
    style Forbidden fill:#FF6B6B
```

---

## Diagram 9: Layers Architecture

Separacja zainteresowań (Separation of Concerns):

```mermaid
graph TB
    subgraph Presentation["👁️ PRESENTATION LAYER"]
        Views["Razor Views"]
        Blazor["Blazor Components"]
    end
    
    subgraph API["🎮 API LAYER"]
        Controllers["Controllers"]
        ActionResults["IActionResult"]
    end
    
    subgraph Business["💼 BUSINESS LOGIC LAYER"]
        Services["Services"]
        LINQ["LINQ Queries"]
        Validation["Validation"]
    end
    
    subgraph Data["💾 DATA ACCESS LAYER"]
        DbContext["DbContext"]
        EFCore["EF Core"]
    end
    
    subgraph Persistence["🗄️ PERSISTENCE LAYER"]
        Database["SQL Server"]
    end
    
    Views -->|Model| Controllers
    Blazor -->|Service Call| Controllers
    Controllers -->|Business Logic| Services
    Services -->|LINQ Query| DbContext
    DbContext -->|SQL| EFCore
    EFCore -->|Execute| Database
    
    style Presentation fill:#FFE4E1
    style API fill:#FFB6C1
    style Business fill:#FFD700
    style Data fill:#87CEEB
    style Persistence fill:#90EE90
```

---

## Diagram 10: Performance Optimization - N+1 vs Include

```mermaid
graph TB
    subgraph N1["❌ N+1 QUERIES (SLOW)"]
        Query1["Query 1:<br/>SELECT * FROM Projects<br/>→ 100 projects"]
        Query2["Query 2-101:<br/>FOR EACH project<br/>SELECT * FROM Tasks WHERE ProjectId=?<br/>→ 100 queries!"]
    end
    
    subgraph Optimized["✅ EAGER LOADING (FAST)"]
        Query3["Query 1:<br/>SELECT * FROM Projects<br/>WITH JOIN Tasks"]
        Query4["Query 2:<br/>SELECT * FROM Tasks<br/>WHERE ProjectId IN (...)"]
    end
    
    N1 -->|Total: 101 queries| Slow["⏱️ ~5000ms"]
    Optimized -->|Total: 2 queries| Fast["⚡ ~50ms"]
    
    Slow -->|100x SLOWER| Problem["❌ Performance Problem"]
    Fast -->|SOLUTION| Solution["✅ Use Include()"]
    
    Solution -->|Code| Code["
    .Include(p => p.Tasks)
    .ToListAsync()
    "]
    
    style N1 fill:#FF6B6B
    style Optimized fill:#90EE90
    style Problem fill:#FF6B6B
    style Solution fill:#90EE90
```

---

## Diagram 11: Caching Strategy

```mermaid
graph TB
    Request1["🌐 Request 1<br/>GET /projects"]
    
    Request1 -->|Check Cache| Cache["💾 Memory Cache"]
    
    Cache -->|Miss| Database1["🗄️ Query DB"]
    Database1 -->|Load| Data["Fetch 100 projects"]
    Data -->|Store| Cache
    Cache -->|Return| Response1["Response<br/>Time: 1000ms"]
    
    Request2["🌐 Request 2<br/>GET /projects<br/>1ms later"]
    
    Request2 -->|Check Cache| Cache
    Cache -->|Hit| Response2["Response<br/>Time: 5ms<br/>200x FASTER!"]
    
    Create["POST /projects<br/>(Create new)"]
    Create -->|Invalidate| CacheInv["🧹 Clear Cache"]
    CacheInv -->|Remove| Cache
    
    style Request1 fill:#E6F3FF
    style Cache fill:#90EE90
    style Database1 fill:#87CEEB
    style Response1 fill:#FFD700
    style Response2 fill:#90EE90
```

---

## Diagram 12: LINQ Query Operations Summary

```mermaid
graph TB
    Data["📊 Data Source"]
    
    subgraph Filtering["🔍 FILTERING"]
        Where["Where()"]
    end
    
    subgraph Sorting["🔄 SORTING"]
        OrderBy["OrderBy()"]
        OrderByDesc["OrderByDescending()"]
        ThenBy["ThenBy()"]
    end
    
    subgraph Grouping["👥 GROUPING"]
        GroupBy["GroupBy()"]
    end
    
    subgraph Projection["📝 PROJECTION"]
        Select["Select()"]
        SelectMany["SelectMany()"]
    end
    
    subgraph Aggregation["➕ AGGREGATION"]
        Count["Count()"]
        Sum["Sum()"]
        Average["Average()"]
        Min["Min() / Max()"]
    end
    
    subgraph Joining["⛓️ JOINING"]
        Join["Join()"]
        GroupJoin["GroupJoin()"]
        Include["Include() - EF Core"]
    end
    
    Data --> Filtering
    Filtering --> Sorting
    Sorting --> Grouping
    Grouping --> Projection
    Projection --> Joining
    Joining --> Aggregation
    Aggregation --> Result["📤 Result"]
    
    style Data fill:#E6F3FF
    style Result fill:#90EE90
    style Filtering fill:#FFE4E1
    style Sorting fill:#FFD700
    style Grouping fill:#FFB6C1
    style Projection fill:#87CEEB
    style Aggregation fill:#F0E68C
```

---

## Diagram 13: Complete Request Response Cycle

End-to-end diagram całego procesu:

```mermaid
graph LR
    User["👤 User<br/>Clicks Link"]
    
    User -->|1| Request["🌐 HTTP Request<br/>GET /projects/123"]
    
    Request -->|2| Pipeline["⚙️ Middleware Pipeline<br/>Logging → Auth → CORS"]
    
    Pipeline -->|3| Router["🗺️ Router<br/>Match Route:<br/>/projects/{id}"]
    
    Router -->|4| Controller["🎮 Controller<br/>ProjectsController<br/>.Details(123)"]
    
    Controller -->|5| Service["🔧 Service<br/>GetProjectAnalyticsAsync(123)"]
    
    Service -->|6| DbContext["💾 DbContext<br/>.Projects.Include()<br/>.FirstOrDefault(p => p.Id == 123)"]
    
    DbContext -->|7| Database["🗄️ SQL Server<br/>SELECT * FROM Projects WHERE Id = 123<br/>SELECT * FROM Tasks WHERE ProjectId = 123"]
    
    Database -->|8| Results["📦 Results<br/>Project object + Tasks collection"]
    
    Results -->|9| Analytics["📊 Analytics Calc<br/>TotalTasks, CompletedTasks, etc."]
    
    Analytics -->|10| ViewModel["📋 ViewModel<br/>ProjectDetailsViewModel"]
    
    ViewModel -->|11| View["👁️ Razor View<br/>Details.cshtml"]
    
    View -->|12| HTML["📄 Render HTML<br/>Bootstrap cards, tables"]
    
    HTML -->|13| Response["📤 HTTP Response<br/>200 OK + HTML"]
    
    Response -->|14| Browser["🌐 Browser<br/>Render & Display"]
    
    Browser -->|15| User
    
    style User fill:#E6F3FF
    style Request fill:#E6F3FF
    style Browser fill:#E6F3FF
    style Controller fill:#FFE4E1
    style Service fill:#FFD700
    style DbContext fill:#87CEEB
    style Database fill:#87CEEB
    style Results fill:#90EE90
    style HTML fill:#90EE90
```

---

## Diagram 14: Comparison Matrix

```mermaid
graph TB
    subgraph ASPCore["ASP.NET Core Features"]
        MVC["✅ MVC Pattern<br/>Controllers → Views"]
        RazorPages["✅ Razor Pages<br/>Simplified"]
        Blazor["✅ Blazor Server<br/>Real-time"]
        API["✅ Web API<br/>REST Endpoints"]
    end
    
    subgraph Alternative["🔄 Comparable Technologies"]
        Spring["Spring Boot<br/>(Java)"]
        Django["Django<br/>(Python)"]
        Rails["Ruby on Rails<br/>(Ruby)"]
        Express["Express.js<br/>(Node.js)"]
    end
    
    subgraph Advantages["🌟 ASP.NET Core Advantages"]
        Performance["⚡ High Performance"]
        Unified["🔗 Unified Platform"]
        DI["💉 Built-in DI"]
        EFCore["🗄️ Entity Framework Core"]
        LINQ["🔍 LINQ"]
    end
    
    ASPCore -->|vs| Alternative
    ASPCore -->|offers| Advantages
    
    style ASPCore fill:#FFE4E1
    style Alternative fill:#E6F3FF
    style Advantages fill:#90EE90
```

---

## Podsumowanie Diagramów

| # | Nazwa | Temat |
|---|-------|-------|
| 1 | Request Flow | HTTP → Controller → Response |
| 2 | Database Schema | Project 1:N Task relacja |
| 3 | DI Container | Service registration & injection |
| 4 | EF Core Pattern | LINQ → SQL → Objects |
| 5 | LINQ Pipeline | Query execution steps |
| 6 | Razor Rendering | .cshtml → HTML |
| 7 | Blazor Lifecycle | Component init & events |
| 8 | Auth Flow | Claim-based authorization |
| 9 | Layers | Separation of concerns |
| 10 | N+1 vs Include | Performance optimization |
| 11 | Caching | Memory cache strategy |
| 12 | LINQ Operations | Query operations summary |
| 13 | Full Cycle | Complete request-response |
| 14 | Comparison | ASP.NET Core vs alternatives |

Wszystkie diagramy wspierają zrozumienie architektury i flow aplikacji! 🎯
