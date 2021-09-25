using Gym2.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2
{
	public class Program
	{
		public static void Main(string[] args)
		{
//			CreateHostBuilder(args).Build().Run();
			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope()) {
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<ApplicationDbContext>();

				//context.Database.EnsureDeleted();
				//context.Database.Migrate();
				//context.Database.EnsureCreated();

				//dotnet user-secrets set "AdminPW" "BytMig123!"
				// OR rightclick on project and choose manage user secrets
				var config = services.GetRequiredService<IConfiguration>();
				var adminPW = config["AdminPW"];

				try
				{
					SeedData.InitAsync(context, services, adminPW).Wait();
				}
				catch (Exception ex)
				{
					throw new ApplicationException($"Seed Failed! {ex.Message}");
				}
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
