using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models
{
	public class GymClass
	{
		public int Id { get; set; }
		
		[Required]
		public string Name { get; set; }
		
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime StartTime { get; set; }

		[Required]
		[DataType(dataType: DataType.Time)]
		public TimeSpan Duration { get; set; }

		public DateTime EndTime { get { return StartTime + Duration; } }

		public String Description { get; set; }

		public ICollection<AppUserGymClass> AppUsers { get; set; }

	}
}
