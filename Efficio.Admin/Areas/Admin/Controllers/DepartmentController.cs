using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Infrastructure.Persistence;

namespace Efficio.Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Departments.Include(d => d.Head).Include(d => d.HeadDepartment);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Head)
                .Include(d => d.HeadDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            ViewData["HeadId"] = new SelectList(_context.Users, "Id", "CountryCode");
            ViewData["HeadDepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,HeadId,HeadDepartmentId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.Id = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeadId"] = new SelectList(_context.Users, "Id", "CountryCode", department.HeadId);
            ViewData["HeadDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.HeadDepartmentId);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["HeadId"] = new SelectList(_context.Users, "Id", "CountryCode", department.HeadId);
            ViewData["HeadDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.HeadDepartmentId);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,HeadId,HeadDepartmentId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["HeadId"] = new SelectList(_context.Users, "Id", "CountryCode", department.HeadId);
            ViewData["HeadDepartmentId"] = new SelectList(_context.Departments, "Id", "Name", department.HeadDepartmentId);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Head)
                .Include(d => d.HeadDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(Guid id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
