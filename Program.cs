// Main namespace references for application dependencies
using EmployeeDirectory.Services;

// Initialize web application builder with default configuration
var builder = WebApplication.CreateBuilder(args);

// Add framework services for MVC with views support
builder.Services.AddControllersWithViews();

// Register employee service as singleton (application-wide instance)
// - Maintains data state throughout application lifetime
builder.Services.AddSingleton<EmployeeService>();

// Configure session management
// - Sessions track individual user state
// - Timeout after 30 minutes of inactivity
// - Mark session cookie as essential for GDPR compliance
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// Enable access to HTTP context in services
// - Required for session state resolution
builder.Services.AddHttpContextAccessor();


var app = builder.Build();


// Production environment hardening
if (!app.Environment.IsDevelopment())
{
    // Custom error handler for production
    app.UseExceptionHandler("/Home/Error");
    
    // Enable HTTP Strict Transport Security
    // - Enforces HTTPS-only connections
    app.UseHsts();
}

// Enable HTTPS redirection for all requests
// - Redirects HTTP to HTTPS automatically
app.UseHttpsRedirection();    
app.UseStaticFiles();         


app.UseSession();

// Routing & Security
app.UseRouting();            
app.UseAuthorization();      



// Default route mapping
// - Fallback to HomeController.Index()
// - Supports optional ID parameter for detail views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start application execution
app.Run();