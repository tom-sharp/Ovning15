using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models.ViewModels
{
	public class BookingToggleViewModel
	{
		public BookingToggleViewModel(AppUser user, bool booked)
		{
			this.Booked = booked;
			this.Email = user.Email;
			this.UserId = user.UserName;
		}
		public bool Booked { get; set; }
		public string Email { get; set; }
		public string UserId { get; set; }
	}
}
