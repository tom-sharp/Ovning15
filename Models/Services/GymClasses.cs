using Gym2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models.Services
{
	public class GymClasses : IGymClasses
	{

		ApplicationDbContext db;

		public GymClasses(ApplicationDbContext context)
		{
			this.db = context;
		}

		public async Task<bool> AddAsync(GymClass gc)
		{
			if (gc != null)
			{
				if (await db.GymClasses.FirstOrDefaultAsync(g => g.Id == gc.Id) == null)
				{
					db.GymClasses.Add(gc);
					try
					{
						await db.SaveChangesAsync();
					}
					catch (DbUpdateException)
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public async Task<bool> RemoveAsync(GymClass gc)
		{
			if (gc != null)
			{
				if (await db.GymClasses.FirstOrDefaultAsync(g => g.Id == gc.Id) != null)
				{
					db.GymClasses.Remove(gc);
					try
					{
						await db.SaveChangesAsync();
					}
					catch (DbUpdateException)
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public async Task<bool> UpdateAsync(GymClass gc)
		{
			if (gc != null)
			{
				if (await db.GymClasses.FirstOrDefaultAsync(g => g.Id == gc.Id) != null)
				{
					db.GymClasses.Update(gc);
					try
					{
						await db.SaveChangesAsync();
					}
					catch (DbUpdateException)
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}


		public async Task<GymClass> GetGymClassAsync(int id)
		{
			if (id <= 0) return null;
			return await db.GymClasses.Include(gc => gc.AppUsers).FirstOrDefaultAsync(gc => gc.Id == id);
		}

		public async Task<bool> AddUserAsync(GymClass gc, AppUser user)
		{
			if ((gc != null) && (user != null))
			{
				if (await db.Participans.FirstOrDefaultAsync(g => g.GymClassId == gc.Id && g.AppUserId == user.Id) == null)
				{
					var book = new AppUserGymClass() { GymClassId = gc.Id, AppUserId = user.Id };
					db.Participans.Add(book);
					try
					{
						await db.SaveChangesAsync();
					}
					catch (DbUpdateException)
					{
						return false;
					}
					return true;
				}
			}
			return false;
		}
		public async Task<bool> RemoveUserAsync(GymClass gc, AppUser user)
		{
			if ((gc != null) && (user != null))
			{
				var unbook = await db.Participans.FirstOrDefaultAsync(g => g.GymClassId == gc.Id && g.AppUserId == user.Id);
				if (unbook != null)
				{
					db.Participans.Remove(unbook);
					try
					{
						await db.SaveChangesAsync();
					}
					catch (DbUpdateException)
					{
						return false;
					}
					return true;
				}
			}
			return false;
		}
	}
}
