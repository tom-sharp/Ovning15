using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models
{
	public class AppUser : IdentityUser
	{
		public ICollection<AppUserGymClass> GymClasses { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return FirstName + " " + LastName; } }
		public DateTime TimeOfRegistration { get; set; }
	}
}
