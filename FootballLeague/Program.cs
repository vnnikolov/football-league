using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballLeague
{
	public class Program
    {
		public static void Main(string[] args)
		{
			IHost host = CreateHostBuilder(args).Build();

			CreateDbIfNotExists(host);

			host.Run();
		}

		private static void CreateDbIfNotExists(IHost host)
		{
			using (IServiceScope scope = host.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;

				try
				{
					Entities.FootballLeagueDbContext context
						= services.GetRequiredService<Entities.FootballLeagueDbContext>();
					context.Database.EnsureCreated();
				}
				catch (Exception ex)
				{
					ILogger logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred creating the database.");
				}
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
