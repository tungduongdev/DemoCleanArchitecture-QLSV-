using Application.UseCases;
using Domain.Interfaces; // Đảm bảo bạn đã khai báo namespace đúng
using Infrastructure.Persistence; // Đây là nơi chứa StudentRepository của bạn
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký AppDbContext với DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the repository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Thêm dịch vụ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(0.5); // Thời gian timeout cho session
    options.Cookie.HttpOnly = true; // Cookie chỉ có thể được truy cập từ phía máy chủ
});
// Add UseCase services
builder.Services.AddScoped<AddStudentUseCase>();
builder.Services.AddScoped<UpdateStudentUseCase>();
builder.Services.AddScoped<DeleteStudentUseCase>();
builder.Services.AddScoped<GetStudentByIdUseCase>();
builder.Services.AddScoped<GetAllStudentsUseCase>();
builder.Services.AddScoped<SearchStudentsByNameUseCase>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    // Kiểm tra nếu yêu cầu đến "/students" mà chưa có Username trong session
    if (context.Request.Path.StartsWithSegments("/students") && !context.Session.TryGetValue("Username", out _))
    {
        context.Response.Redirect("/user/login");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");


app.Run();
