// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class StockReceiptController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public StockReceiptController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: StockReceipt
//         public async Task<IActionResult> Index()
//         {
//             var appDbContext = _context.StockReceipts.Include(s => s.Distributor).Include(s => s.Stock).Include(s => s.StockOrder);
//             return View(await appDbContext.ToListAsync());
//         }
//
//         // GET: StockReceipt/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockReceipt = await _context.StockReceipts
//                 .Include(s => s.Distributor)
//                 .Include(s => s.Stock)
//                 .Include(s => s.StockOrder)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stockReceipt == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stockReceipt);
//         }
//
//         // GET: StockReceipt/Create
//         public IActionResult Create()
//         {
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name");
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name");
//             ViewData["StockOrderId"] = new SelectList(_context.StockOrders, "Id", "Id");
//             return View();
//         }
//
//         // POST: StockReceipt/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("MadeBy,ReceivedBy,StockOrderId,DistributorId,StockId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] StockReceipt stockReceipt)
//         {
//             if (ModelState.IsValid)
//             {
//                 stockReceipt.Id = Guid.NewGuid();
//                 _context.Add(stockReceipt);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockReceipt.DistributorId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockReceipt.StockId);
//             ViewData["StockOrderId"] = new SelectList(_context.StockOrders, "Id", "Id", stockReceipt.StockOrderId);
//             return View(stockReceipt);
//         }
//
//         // GET: StockReceipt/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockReceipt = await _context.StockReceipts.FindAsync(id);
//             if (stockReceipt == null)
//             {
//                 return NotFound();
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockReceipt.DistributorId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockReceipt.StockId);
//             ViewData["StockOrderId"] = new SelectList(_context.StockOrders, "Id", "Id", stockReceipt.StockOrderId);
//             return View(stockReceipt);
//         }
//
//         // POST: StockReceipt/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("MadeBy,ReceivedBy,StockOrderId,DistributorId,StockId,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] StockReceipt stockReceipt)
//         {
//             if (id != stockReceipt.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(stockReceipt);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!StockReceiptExists(stockReceipt.Id))
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
//             ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", stockReceipt.DistributorId);
//             ViewData["StockId"] = new SelectList(_context.Stocks, "Id", "Name", stockReceipt.StockId);
//             ViewData["StockOrderId"] = new SelectList(_context.StockOrders, "Id", "Id", stockReceipt.StockOrderId);
//             return View(stockReceipt);
//         }
//
//         // GET: StockReceipt/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var stockReceipt = await _context.StockReceipts
//                 .Include(s => s.Distributor)
//                 .Include(s => s.Stock)
//                 .Include(s => s.StockOrder)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (stockReceipt == null)
//             {
//                 return NotFound();
//             }
//
//             return View(stockReceipt);
//         }
//
//         // POST: StockReceipt/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var stockReceipt = await _context.StockReceipts.FindAsync(id);
//             if (stockReceipt != null)
//             {
//                 _context.StockReceipts.Remove(stockReceipt);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
         // private bool StockReceiptExists(Guid id)
//         {
//             return _context.StockReceipts.Any(e => e.Id == id);
//         }
//     }
// }
