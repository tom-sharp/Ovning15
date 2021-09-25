using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models.ViewModels
{
	public class UserDetailsViewModel
	{
		public UserDetailsViewModel(AppUser user)
		{
			if (user != null) {
				UserId = user.Id;
				UserName = user.UserName;
				PasswordHash = user.PasswordHash;
				Email = user.Email;
				EmailConfirmed = user.EmailConfirmed;
				AccessFailedCount = user.AccessFailedCount;
				TwoFactorEnabled = user.TwoFactorEnabled;
				LockoutEnabled = user.LockoutEnabled;
				LockoutEnd = user.LockoutEnd;
				ConcurrencyStamp = user.ConcurrencyStamp;   // random Value written each time record updates
				SecurityStamp = user.SecurityStamp;         // random value written each time user cred change
				NormalizedEmail = user.NormalizedEmail;
				NormalizedUserName = user.NormalizedUserName;
			}
		}
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string NormalizedUserName { get; set; }
		public string PasswordHash { get; set; }
		public string ConcurrencyStamp { get; set; }
		public string SecurityStamp { get; set; }
		public string Email { get; set; }
		public string NormalizedEmail { get; set; }
		public bool EmailConfirmed { get; set; }
		public int AccessFailedCount { get; set; }
		public bool TwoFactorEnabled { get; set; }
		public bool LockoutEnabled { get; set; }
		public DateTimeOffset? LockoutEnd { get; set; }

	}
}
