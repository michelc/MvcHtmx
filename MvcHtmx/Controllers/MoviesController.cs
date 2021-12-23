using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcHtmx.Models;

namespace MvcHtmx.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcHtmxContext _context;

        public MoviesController(MvcHtmxContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var model = await (from x in _context.Movies
                               orderby x.Title
                               select new MovieViewModel
                               {
                                   Movie_ID = x.Movie_ID,
                                   Title = x.Title,
                                   Year = x.ReleaseDate.Year,
                                   Genre = x.Genre,
                                   Price = x.Price
                               }).ToListAsync();

            return View(model);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var model = await GetMovieDisplayViewModel(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            var model = new MovieEditorViewModel();

            return View(model);
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieEditorViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model.ToMovie());
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var model = await GetMovieEditorViewModel(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieEditorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model.ToMovie());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(model.Movie_ID))
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

            return View(model);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var model = await GetMovieDisplayViewModel(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var model = await _context.Movies.FindAsync(id);
                _context.Movies.Remove(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch { }

            return View(await GetMovieDisplayViewModel(id));
        }

        private async Task<MovieDisplayViewModel> GetMovieDisplayViewModel(int? id)
        {
            if (id == null) return null;

            var model = await (from x in _context.Movies
                               where x.Movie_ID == id
                               select new MovieDisplayViewModel
                               {
                                   Movie_ID = x.Movie_ID,
                                   Title = x.Title,
                                   ReleaseDate = x.ReleaseDate,
                                   Genre = x.Genre,
                                   Price = x.Price
                               }).FirstOrDefaultAsync();

            return model;
        }

        private async Task<MovieEditorViewModel> GetMovieEditorViewModel(int? id)
        {
            if (id == null) return null;

            var model = await (from x in _context.Movies
                               where x.Movie_ID == id
                               select new MovieEditorViewModel
                               {
                                   Movie_ID = x.Movie_ID,
                                   Title = x.Title,
                                   ReleaseDate = x.ReleaseDate,
                                   Genre = x.Genre,
                                   Price = x.Price
                               }).FirstOrDefaultAsync();

            return model;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Movie_ID == id);
        }
    }
}
