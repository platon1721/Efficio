// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.IMS.Common;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     public class ProductTypeController : Controller
//     {
//         private readonly AppDbContext _context;
//
//         public ProductTypeController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: ProductType
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.ProductTypes.ToListAsync());
//         }
//
//         // GET: ProductType/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var productType = await _context.ProductTypes
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (productType == null)
//             {
//                 return NotFound();
//             }
//
//             return View(productType);
//         }
//
//         // GET: ProductType/Create
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: ProductType/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Title,Description,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] ProductType productType)
//         {
//             if (ModelState.IsValid)
//             {
//                 productType.Id = Guid.NewGuid();
//                 _context.Add(productType);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(productType);
//         }
//
//         // GET: ProductType/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var productType = await _context.ProductTypes.FindAsync(id);
//             if (productType == null)
//             {
//                 return NotFound();
//             }
//             return View(productType);
//         }
//
//         // POST: ProductType/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] ProductType productType)
//         {
//             if (id != productType.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(productType);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!ProductTypeExists(productType.Id))
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
//             return View(productType);
//         }
//
//         // GET: ProductType/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var productType = await _context.ProductTypes
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (productType == null)
//             {
//                 return NotFound();
//             }
//
//             return View(productType);
//         }
//
//         // POST: ProductType/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var productType = await _context.ProductTypes.FindAsync(id);
//             if (productType != null)
//             {
//                 _context.ProductTypes.Remove(productType);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool ProductTypeExists(Guid id)
//         {
//             return _context.ProductTypes.Any(e => e.Id == id);
//         }
//     }
// }
