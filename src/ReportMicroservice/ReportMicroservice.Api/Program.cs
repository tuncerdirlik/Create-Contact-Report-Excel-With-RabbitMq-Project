using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ReportMicroservice.Api.DbContexts;
using ReportMicroservice.Api.Repositories;
using ReportMicroservice.Api.Repositories.Contracts;
using ReportMicroservice.Api.Services;
using ReportMicroservice.Api.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    HostName = builder.Configuration.GetSection("RabbitMqConnection:Host").Get<string>(),
    Port = builder.Configuration.GetSection("RabbitMqConnection:Port").Get<int>(),
    DispatchConsumersAsync = true
});

builder.Services.AddSingleton<RabbitMqClientServices>();
builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IReportFileRepository, ReportFileRepository>();

builder.Services.AddHttpClient<IContactService, ContactService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
