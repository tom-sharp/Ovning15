using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gym2.Data;
using Gym2.Models;
using Gym2.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Gym2.Models.Services;

namespace Gym2.Controllers
{
	public class GymClassesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> usrMgr;

		public GymClassesController(ApplicationDbContext context, UserManager<AppUser> usrmgr)
		{
			_context = context;
			usrMgr = usrmgr;
		}

		// GET: GymClasses
		public async Task<IActionResult> Index()
		{
			IQueryable<GymClassesViewModel> model;
			var user = await usrMgr.GetUserAsync(User);
			if (User.Identity.IsAuthenticated) {
				model = _context.GymClasses.Include(g => g.AppUsers).Select(g => new GymClassesViewModel(g, user));
			}
			else model = _context.GymClasses.Select(g => new GymClassesViewModel(g, null));
			return View(await model.ToListAsync());
		}

		// GET: GymClasses/Details/5
		[Authorize]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var gymClass = await _context.GymClasses.Include(g=> g.AppUsers).ThenInclude(g=> g.AppUser).FirstOrDefaultAsync(ag => ag.Id == id);
			if (gymClass == null) return NotFound();

			return View(gymClass);
		}

		// GET: GymClasses/Create
		[Authorize("IsAdmin")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: GymClasses/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize("IsAdmin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
		{
			if (ModelState.IsValid)
			{
				_context.Add(gymClass);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(gymClass);
		}

		// GET: GymClasses/Edit/5
		[Authorize("IsAdmin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var gymClass = await _context.GymClasses.FindAsync(id);
			if (gymClass == null)
			{
				return NotFound();
			}
			return View(gymClass);
		}

		// POST: GymClasses/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize("IsAdmin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
		{
			if (id != gymClass.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(gymClass);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!GymClassExists(gymClass.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(gymClass);
		}

		// GET: GymClasses/Delete/5
		[Authorize("IsAdmin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var gymClass = await _context.GymClasses
				.FirstOrDefaultAsync(m => m.Id == id);
			if (gymClass == null)
			{
				return NotFound();
			}

			return View(gymClass);
		}

		// POST: GymClasses/Delete/5
		[Authorize("IsAdmin")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var gymClass = await _context.GymClasses.FindAsync(id);
			_context.GymClasses.Remove(gymClass);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize]
		public async Task<IActionResult> BookingToggle(int? id) {
			if (id == null) return RedirectToAction("Index");
			if (await _context.GymClasses.FirstOrDefaultAsync(g => g.Id == (int)id) == null) return RedirectToAction("Index");
			var userid = usrMgr.GetUserId(User);
			var participate = await _context.Participans.FirstOrDefaultAsync(ag=> ag.GymClassId == id && ag.AppUserId == userid);
			if (participate == null)
			{
				_context.Participans.Add(new AppUserGymClass { AppUserId = userid, GymClassId = (int)id });
			}
			else {
				_context.Participans.Remove(participate);
			}
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[Authorize]
		public async Task<IActionResult> BookedPasses()
		{
			List<GymClassesViewModel> model;
			var user = await usrMgr.GetUserAsync(User);
			if ((User.Identity.IsAuthenticated) && (user != null))
			{
				model = await _context.GymClasses.Include(g => g.AppUsers).Where(gc=> gc.StartTime > DateTime.Now).Select(g => new GymClassesViewModel(g, user)).ToListAsync();
			}
			else return RedirectToAction("Index");
			return View(model.Where(b=> b.UserBooked).ToList());
		}

		[Authorize]
		public async Task<IActionResult> PassedPasses()
		{
			List<GymClassesViewModel> model;
			var user = await usrMgr.GetUserAsync(User);
			if ((User.Identity.IsAuthenticated) && (user != null))
			{
				model = await _context.GymClasses.Include(g => g.AppUsers).Where(gc => gc.StartTime < DateTime.Now).Select(g => new GymClassesViewModel(g, user)).ToListAsync();
			}
			else return RedirectToAction("Index");
			return View(model.Where(b => b.UserBooked).ToList());
		}

		public async Task<IActionResult> UserDetails()
		{
			var user = await usrMgr.GetUserAsync(User);
			return View(new UserDetailsViewModel(user));
		}

		[Authorize("IsAdmin")]
		public async Task<IActionResult> Users()
		{
			var model = _context.Users.Select(u => new UserDetailsViewModel(u));
			return View(await model.ToListAsync());
		}

		private bool GymClassExists(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }


    }
}
