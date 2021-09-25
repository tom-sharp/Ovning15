using Gym2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gym2.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AppUserGymClass>().HasKey(ag => new { ag.AppUserId, ag.GymClassId });
		}

		public DbSet<GymClass> GymClasses { get; set; }
//		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppUserGymClass> Participans { get; set; }

	}

}
