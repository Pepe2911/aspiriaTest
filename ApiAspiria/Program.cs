using ApiAspiria.Models;
using ApiAspiria.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => x.AddPolicy("EnableCors", build =>
{
    build.SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

builder.Services.AddDbContext<AspiriaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AspiriaConnection"));
});

builder.Services.AddTransient<IJugueteRepository, JugueteRepository>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AspiriaContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("EnableCors");

app.MapControllers();

app.Run();
