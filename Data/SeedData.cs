using Gym2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Data
{
	public static class SeedData {
		private static ApplicationDbContext db;
		private static RoleManager<IdentityRole> roleManager;
		private static UserManager<AppUser> userManager;

		public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string adminPW) {
			db = context;
			if (db == null) throw new ApplicationException("SeedInitAsync:: DBcontext null argument");
			if ((adminPW == null) || adminPW.Length == 0) throw new ApplicationException("SeedInitAsync:: AdminPW is null or empty");

			roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
			if (roleManager == null) throw new ApplicationException("SeedInitAsync:: Cant get service RoleManager");

			userManager = services.GetRequiredService<UserManager<AppUser>>();
			if (userManager is null) throw new ApplicationException("SeedInitAsync:: Cant get service UserManager");

			// Create Admin user
			string adminEmail = "admin@gym2.se";
			AppUser admin = await userManager.FindByEmailAsync(adminEmail);

			if (admin == null) {
				admin = new AppUser() { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true, FirstName = "Admin", LastName = "Admin", TimeOfRegistration = DateTime.Now };
				var res = await userManager.CreateAsync(admin, adminPW);
				if (!res.Succeeded) throw new ApplicationException("SeedInitAsync:: Could not create Admin account");
			}

			// Create Roles
			string[] roles = { "User", "Admin" };
			foreach (var role in roles) {
				var r = await roleManager.FindByNameAsync(role);
				if (r == null) {
					r = new IdentityRole() { Name = role};
					var res = await roleManager.CreateAsync(r);
					if (!res.Succeeded) throw new ApplicationException("SeedInitAsync:: Coluld not create Role");
				}
			}

			// Add all Roles to admin
			foreach (var role in roles) {
				if (await userManager.IsInRoleAsync(admin, role) == false) {
					var res = userManager.AddToRoleAsync(admin, role);
					if (!res.Result.Succeeded) throw new ApplicationException("SeedInitAsync:: Could not Add Admin to Role");
				}
			}

			await db.SaveChangesAsync();
		}


	}
}
