var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();// Add Razor Pages services to the service collection, AddRazorPages is used to add Razor Pages services to the service collection, which is needed for the application to work correctly.
builder.Services.AddServerSideBlazor();// Add Server-Side Blazor services to the service collection, AddServerSideBlazor is used to add Server-Side Blazor services to the service collection, which is needed for the application to work correctly.
//builder.Services.AddRazorComponents();// Add Razor Components services to the service collection, AddRazorComponents is used to add Razor Components services to the service collection, which is needed for the application to work correctly. If I am using AddServerSideBlazor or AddRazorPages, I don't need to call this method explicitly.


builder.Services.AddScoped<TicketmasterService>();// Register service
//registers an HttpClient specifically configured for TicketmasterService.
builder.Services.AddHttpClient<TicketmasterService>(client =>
{
    var baseUrl = builder.Configuration.GetSection("TicketMasterApiDevBaseUrl").Value;
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("TicketMasterApiDevBaseUrl is not configured.");
    }
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30); // Set timeout to 30 seconds to avoid long waiting times for the response
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
