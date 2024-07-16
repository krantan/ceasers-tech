using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.Business.Commands;
using api.Business.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GuestContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("GuestApiDatabase")));

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateGuestPreProcessor>();
    cfg.AddRequestPreProcessor<CreateAccountPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GuestContext>();
    //context.Database.EnsureDeleted(); //For deleting and Re-Seedng the Database
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();
app.UseCookiePolicy();

app.Run();
