var builder = WebApplication.CreateBuilder(args);

// MVC Servislerini Ekle
builder.Services.AddControllersWithViews();

// --- GATEWAY BAÐLANTISI ---
// Controller'larda IHttpClientFactory kullanarak çaðýracaðýz
builder.Services.AddHttpClient("Gateway", client =>
{
  client.BaseAddress = new Uri("https://localhost:5000"); // Gateway Portu
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota Tanýmý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();