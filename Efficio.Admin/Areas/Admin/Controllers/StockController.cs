// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.Common;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class StockController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public StockController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: Stock
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.Stocks.ToListAsync());
//         }
//
//         // GET: Stock/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stock = await _context.Stocks
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stock == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stock);
//         }
//
//         // GET: Stock/Create
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: Stock/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Stock stock)
//         {
//             if (ModelState.IsValid)
//             {
//                 stock.Id = Guid.NewGuid();
//                 _context.Add(stock);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(stock);
//         }
//
//         // GET: Stock/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stock = await _context.Stocks.FindAsync(id);
//             if (stock == null)
//             {
//                 return NotFound();
//             }
//             return View(stock);
//         }
//
//         // POST: Stock/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("Name,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Stock stock)
//         {
//             if (id != stock.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(stock);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!StockExists(stock.Id))
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
//             return View(stock);
//         }
//
//         // GET: Stock/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stock = await _context.Stocks
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stock == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stock);
//         }
//
//         // POST: Stock/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var stock = await _context.Stocks.FindAsync(id);
//             if (stock != null)
//             {
//                 _context.Stocks.Remove(stock);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool StockExists(Guid id)
//         {
//             return _context.Stocks.Any(e => e.Id == id);
//         }
//     }
// }
