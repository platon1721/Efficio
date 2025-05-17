// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class SalesRepController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public SalesRepController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: SalesRep
//         public async Task<IActionResult> Index()
//         {
//             var appDbContext = _context.SalesReps.Include(s => s.Distributor);
//             return View(await appDbContext.ToListAsync());
//         }
//
//         // GET: SalesRep/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var salesRep = await _context.SalesReps
//                 .Include(s => s.Distributor)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (salesRep == null)
//             {
//                 return NotFound();
//             }
//
//             return View(salesRep);
//         }
//
//         // GET: SalesRep/Create
//         public IActionResult Create()
//         {
//             ViewData["DistributorId"] = new SelectList(_context.Distributor, "Id", "Name");
//             return View();
//         }
//
//         // POST: SalesRep/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("DistributorId,FirstName,SurName,Email,CountryCode,Number,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] SalesRep salesRep)
//         {
//             if (ModelState.IsValid)
//             {
//                 salesRep.Id = Guid.NewGuid();
//                 _context.Add(salesRep);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", salesRep.DistributorId);
//             return View(salesRep);
//         }
//
//         // GET: SalesRep/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var salesRep = await _context.SalesReps.FindAsync(id);
//             if (salesRep == null)
//             {
//                 return NotFound();
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", salesRep.DistributorId);
//             return View(salesRep);
//         }
//
//         // POST: SalesRep/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("DistributorId,FirstName,SurName,Email,CountryCode,Number,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] SalesRep salesRep)
//         {
//             if (id != salesRep.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(salesRep);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!SalesRepExists(salesRep.Id))
//                     {
//                         return NotFound();
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", salesRep.DistributorId);
//             return View(salesRep);
//         }
//
//         // GET: SalesRep/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var salesRep = await _context.SalesReps
//                 .Include(s => s.Distributor)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (salesRep == null)
//             {
//                 return NotFound();
//             }
//
//             return View(salesRep);
//         }
//
//         // POST: SalesRep/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var salesRep = await _context.SalesReps.FindAsync(id);
//             if (salesRep != null)
//             {
//                 _context.SalesReps.Remove(salesRep);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool SalesRepExists(Guid id)
//         {
//             return _context.SalesReps.Any(e => e.Id == id);
//         }
//     }
// }
