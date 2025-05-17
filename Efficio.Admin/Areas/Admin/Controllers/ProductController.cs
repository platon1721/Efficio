// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.Common;
// using Efficio.Core.Domain.Entities.IMS.WMS;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class ProductController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public ProductController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: Product
//         public async Task<IActionResult> Index()
//         {
//             var appDbContext = _context.Products.Include(p => p.Distributor);
//             return View(await appDbContext.ToListAsync());
//         }
//
//         // GET: Product/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var product = await _context.Products
//                 .Include(p => p.Distributor)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (product == null)
//             {
//                 return NotFound();
//             }
//
//             return View(product);
//         }
//
//         // GET: Product/Create
//         public IActionResult Create()
//         {
//             ViewData["DistributorId"] = new SelectList(_context.Set<Distributor>(), "Id", "Name");
//             return View();
//         }
//
//         // POST: Product/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Title,Description,DistributorId,LargeImagePath,ButtonImagePath,ImageSize,BaseUnit,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Product product)
//         {
//             if (ModelState.IsValid)
//             {
//                 product.Id = Guid.NewGuid();
//                 _context.Add(product);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Set<Distributor>(), "Id", "Name", product.DistributorId);
//             return View(product);
//         }
//
//         // GET: Product/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var product = await _context.Products.FindAsync(id);
//             if (product == null)
//             {
//                 return NotFound();
//             }
//             ViewData["DistributorId"] = new SelectList(_context.Set<Distributor>(), "Id", "Name", product.DistributorId);
//             return View(product);
//         }
//
//         // POST: Product/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,DistributorId,LargeImagePath,ButtonImagePath,ImageSize,BaseUnit,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] Product product)
//         {
//             if (id != product.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(product);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!ProductExists(product.Id))
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
//             ViewData["DistributorId"] = new SelectList(_context.Set<Distributor>(), "Id", "Name", product.DistributorId);
//             return View(product);
//         }
//
//         // GET: Product/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var product = await _context.Products
//                 .Include(p => p.Distributor)
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (product == null)
//             {
//                 return NotFound();
//             }
//
//             return View(product);
//         }
//
//         // POST: Product/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var product = await _context.Products.FindAsync(id);
//             if (product != null)
//             {
//                 _context.Products.Remove(product);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool ProductExists(Guid id)
//         {
//             return _context.Products.Any(e => e.Id == id);
//         }
//     }
// }
