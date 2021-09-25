using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models
{
	public class AppUserGymClass
	{
		public string AppUserId { get; set; }
		public int GymClassId { get; set; }

		public AppUser AppUser { get; set; }
		public GymClass GymClass { get; set; }

	}
}
