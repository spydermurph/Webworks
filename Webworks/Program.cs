using Microsoft.EntityFrameworkCore;
using Webworks.Context;
using Webworks.Contracts;
using Webworks.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebworksContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebworksContext"));
});

// Add services to the container.

builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
