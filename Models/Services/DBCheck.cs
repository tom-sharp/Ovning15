using Gym2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym2.Models.Services
{
	public class DBCheck
	{
		readonly ApplicationDbContext db;
		public DBCheck(ApplicationDbContext context)
		{
			this.db = context;
		}

	}
}
