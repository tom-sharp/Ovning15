using System.Threading.Tasks;

namespace Gym2.Models.Services
{
	public interface IGymClasses
	{
		Task<bool> AddAsync(GymClass gc);
		Task<bool> AddUserAsync(GymClass gc, AppUser user);
		Task<GymClass> GetGymClassAsync(int id);
		Task<bool> RemoveAsync(GymClass gc);
		Task<bool> RemoveUserAsync(GymClass gc, AppUser user);
		Task<bool> UpdateAsync(GymClass gc);
	}
}