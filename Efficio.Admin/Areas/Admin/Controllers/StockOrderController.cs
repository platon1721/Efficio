// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class StockOrderController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public StockOrderController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: StockOrder
//         public async Task<IActionResult> Index()
//         {
//             var appDbContext = _context.StockOrders.Include(s => s.Distributor).Include(s => s.SalesRep).Include(s => s.Stock);
//             return View(await appDbContext.ToListAsync());
//         }
//
//         // GET: StockOrder/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockOrder = await _context.StockOrders
//                 .Include(s => s.Distributor)
//                 .Include(s => s.SalesRep)
//                 .Include(s => s.Stock)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stockOrder == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stockOrder);
//         }
//
//         // GET: StockOrder/Create
//         public IActionResult Create()
//         {
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name");
//             ViewData["SalesRepId"] = new SelectList(_context.SalesReps, "Id", "CountryCode");
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name");
//             return View();
//         }
//
//         // POST: StockOrder/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Status,MadeBy,StockId,DistributorId,SalesRepId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] StockOrder stockOrder)
//         {
//             if (ModelState.IsValid)
//             {
//                 stockOrder.Id = Guid.NewGuid();
//                 _context.Add(stockOrder);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockOrder.DistributorId);
//             ViewData["SalesRepId"] = new SelectList(_context.SalesReps, "Id", "CountryCode", stockOrder.SalesRepId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockOrder.StockId);
//             return View(stockOrder);
//         }
//
//         // GET: StockOrder/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockOrder = await _context.StockOrders.FindAsync(id);
//             if (stockOrder == null)
//             {
//                 return NotFound();
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockOrder.DistributorId);
//             ViewData["SalesRepId"] = new SelectList(_context.SalesReps, "Id", "CountryCode", stockOrder.SalesRepId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockOrder.StockId);
//             return View(stockOrder);
//         }
//
//         // POST: StockOrder/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("Status,MadeBy,StockId,DistributorId,SalesRepId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] StockOrder stockOrder)
//         {
//             if (id != stockOrder.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(stockOrder);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!StockOrderExists(stockOrder.Id))
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
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockOrder.DistributorId);
//             ViewData["SalesRepId"] = new SelectList(_context.SalesReps, "Id", "CountryCode", stockOrder.SalesRepId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockOrder.StockId);
//             return View(stockOrder);
//         }
//
//         // GET: StockOrder/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockOrder = await _context.StockOrders
//                 .Include(s => s.Distributor)
//                 .Include(s => s.SalesRep)
//                 .Include(s => s.Stock)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stockOrder == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stockOrder);
//         }
//
//         // POST: StockOrder/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var stockOrder = await _context.StockOrders.FindAsync(id);
//             if (stockOrder != null)
//             {
//                 _context.StockOrders.Remove(stockOrder);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool StockOrderExists(Guid id)
//         {
//             return _context.StockOrders.Any(e => e.Id == id);
//         }
//     }
// }
