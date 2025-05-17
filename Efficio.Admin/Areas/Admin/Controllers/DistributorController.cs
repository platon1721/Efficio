// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class DistributorController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public DistributorController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: Distributor
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.Distributors.ToListAsync());
//         }
//
//         // GET: Distributor/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var distributor = await _context.Distributors
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (distributor == null)
//             {
//                 return NotFound();
//             }
//
//             return View(distributor);
//         }
//
//         // GET: Distributor/Create
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: Distributor/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Distributor distributor)
//         {
//             if (ModelState.IsValid)
//             {
//                 distributor.Id = Guid.NewGuid();
//                 _context.Add(distributor);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(distributor);
//         }
//
//         // GET: Distributor/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var distributor = await _context.Distributors.FindAsync(id);
//             if (distributor == null)
//             {
//                 return NotFound();
//             }
//             return View(distributor);
//         }
//
//         // POST: Distributor/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("Name,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Distributor distributor)
//         {
//             if (id != distributor.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(distributor);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!DistributorExists(distributor.Id))
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
//             return View(distributor);
//         }
//
//         // GET: Distributor/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var distributor = await _context.Distributors
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (distributor == null)
//             {
//                 return NotFound();
//             }
//
//             return View(distributor);
//         }
//
//         // POST: Distributor/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var distributor = await _context.Distributors.FindAsync(id);
//             if (distributor != null)
//             {
//                 _context.Distributors.Remove(distributor);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool DistributorExists(Guid id)
//         {
//             return _context.Distributors.Any(e => e.Id == id);
//         }
//     }
// }
