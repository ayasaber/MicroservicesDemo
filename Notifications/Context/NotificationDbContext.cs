using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.Entities;

namespace Notifications.Context
{
	public class NotificationDbContext : DbContext
	{
		private readonly IConfiguration Configuration;
		public NotificationDbContext() { }
		public NotificationDbContext(DbContextOptions<NotificationDbContext> options, IConfiguration configuration) : base(options) 
		{
			Configuration = configuration;
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json")
			  .Build();

				optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConnections"));
			}
		}
		public DbSet<User> Users { get; set; }
        public DbSet<NotificationSeverity> NotificationSeverities { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<EmailLogs> EmailLogs { get; set; }
        public DbSet<SMSLogs> SMSLogs { get; set; }
        public DbSet<WebPushLogs> WebPushLogs { get; set; }

    }
}
