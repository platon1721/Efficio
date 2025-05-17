// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class WriteOffController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public WriteOffController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: WriteOff
//         public async Task<IActionResult> Index()
//         {
//             var appDbContext = _context.WriteOffs.Include(w => w.Stock);
//             return View(await appDbContext.ToListAsync());
//         }
//
//         // GET: WriteOff/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var writeOff = await _context.WriteOffs
//                 .Include(w => w.Stock)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (writeOff == null)
//             {
//                 return NotFound();
//             }
//
//             return View(writeOff);
//         }
//
//         // GET: WriteOff/Create
//         public IActionResult Create()
//         {
//             ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name");
//             return View();
//         }
//
//         // POST: WriteOff/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("MadeBy,StockId,Reason,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] WriteOff writeOff)
//         {
//             if (ModelState.IsValid)
//             {
//                 writeOff.Id = Guid.NewGuid();
//                 _context.Add(writeOff);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", writeOff.StockId);
//             return View(writeOff);
//         }
//
//         // GET: WriteOff/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var writeOff = await _context.WriteOffs.FindAsync(id);
//             if (writeOff == null)
//             {
//                 return NotFound();
//             }
//             ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", writeOff.StockId);
//             return View(writeOff);
//         }
//
//         // POST: WriteOff/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("MadeBy,StockId,Reason,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] WriteOff writeOff)
//         {
//             if (id != writeOff.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(writeOff);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!WriteOffExists(writeOff.Id))
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
//             ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", writeOff.StockId);
//             return View(writeOff);
//         }
//
//         // GET: WriteOff/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var writeOff = await _context.WriteOffs
//                 .Include(w => w.Stock)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (writeOff == null)
//             {
//                 return NotFound();
//             }
//
//             return View(writeOff);
//         }
//
//         // POST: WriteOff/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var writeOff = await _context.WriteOffs.FindAsync(id);
//             if (writeOff != null)
//             {
//                 _context.WriteOffs.Remove(writeOff);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool WriteOffExists(Guid id)
//         {
//             return _context.WriteOffs.Any(e => e.Id == id);
//         }
//     }
// }
