using Microsoft.EntityFrameworkCore;
using School.Domain.Data;
using Common.Repository.IRepository;
using Common.Repository.Repository;
using Common.Service.IServices;
using School.Service.Services;
using System.Reflection;
using Logging.Services;
using Serilog;
using Caching;
using Caching.Redis;
using Microsoft.Extensions.Configuration;
using Notifications.Context;
using Notifications.Services;
using Notifications.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("DBConnections");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service Injection
builder.Services.AddScoped<ICacheManager, CacheManager>();
builder.Services.AddScoped(typeof(IRepository<School.Domain.Models.School>), typeof(Repository<School.Domain.Models.School, SchoolDbContext>));
builder.Services.AddScoped<IBaseService<School.Domain.Models.School>,SchoolService>();
builder.Services.AddScoped<NotificationDbContext>();
builder.Services.AddScoped<ISMSLogsService, SMSLogsService>();
builder.Services.AddScoped<IEmailLogsService, EmailLogsService>();
builder.Services.AddScoped<IWebPushLogsService, WebPushLogsService>();
builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddScoped<EmailSettings>();
builder.Services.AddScoped<NotificationService>();

//builder.Services.AddDbContextPool<NotificationDbContext>(
//                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnections")));

builder.Services.AddDbContext<SchoolDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnections")));

builder.Host.UseSerilog();


var app = builder.Build();
ElasticsearchLogging.AddConfiguration(Assembly.GetExecutingAssembly().GetName().Name!);

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
