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
