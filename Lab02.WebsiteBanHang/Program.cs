using Lab02.WebsiteBanHang.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// _products, _categories khởi tạo trong ctor
// request -> tạo mới 1 instance của ProductRepository, CategoryRepository

// builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
// builder.Services.AddScoped<IProductRepository, MockProductRepository>();

// -> dùng Singleton để tạo 1 instance duy nhất cho cả ứng dụng
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddSingleton<ICategoryRepository, MockCategoryRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();   

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
