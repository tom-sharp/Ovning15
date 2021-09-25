using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models.ViewModels
{
	public class GymClassesViewModel
	{
		public GymClassesViewModel(GymClass gc, AppUser user)
		{
			if (gc != null) {
				this.Id = gc.Id;
				this.Name = gc.Name;
				this.StartTime = gc.StartTime;
				this.Duration = gc.Duration;
				this.Description = gc.Description;
				this.UserBooked = false;
				if ((user != null) && (gc.AppUsers.FirstOrDefault(au => au.AppUserId == user.Id) != null)) this.UserBooked = true;
			}
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public TimeSpan Duration { get; set; }

		public String Description { get; set; }

		public DateTime EndTime { get { return StartTime + Duration; } }

		public bool UserBooked { get; set; }

	}
}
