// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Efficio.Core.Domain.Entities.Common;
// using Efficio.Infrastructure.Persistence;
//
// namespace Efficio.Admin.Areas.Admin.Controllers
// {
//     [Area("Admin")]
//     public class UserController : AdminBaseController
//     {
//         private readonly AppDbContext _context;
//
//         public UserController(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: Admin/User
//         public async Task<IActionResult> Index()
//         {
//             return View(await _context.Users.ToListAsync());
//         }
//
//         // GET: Admin/User/Details/5
//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var user = await _context.Users
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//
//             return View(user);
//         }
//
//         // GET: Admin/User/Create
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: Admin/User/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("FirstName,SurName,Email,CountryCode,Number,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] User user)
//         {
//             if (ModelState.IsValid)
//             {
//                 user.Id = Guid.NewGuid();
//                 _context.Add(user);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(user);
//         }
//
//         // GET: Admin/User/Edit/5
//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var user = await _context.Users.FindAsync(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//             return View(user);
//         }
//
//         // POST: Admin/User/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,SurName,Email,CountryCode,Number,IsDeleted,DeletedBy,DeletedAt,Id,CreatedAt,ModifiedAt")] User user)
//         {
//             if (id != user.Id)
//             {
//                 return NotFound();
//             }
//
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(user);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!UserExists(user.Id))
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
//             return View(user);
//         }
//
//         // GET: Admin/User/Delete/5
//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//
//             var user = await _context.Users
//                 .FirstOrDefaultAsync(m => m.Id == id);
//             if (user == null)
//             {
//                 return NotFound();
//             }
//
//             return View(user);
//         }
//
//         // POST: Admin/User/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(Guid id)
//         {
//             var user = await _context.Users.FindAsync(id);
//             if (user != null)
//             {
//                 _context.Users.Remove(user);
//             }
//
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         private bool UserExists(Guid id)
//         {
//             return _context.Users.Any(e => e.Id == id);
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Efficio.Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : AdminBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<UserController> _localizer;

        public UserController(IUnitOfWork unitOfWork, IStringLocalizer<UserController> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return View(users);
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/User/Create
        public IActionResult Create()
        {
            // Initialize a new user with default values
            var user = new User
            {
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            return View(user);
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,SurName,Email,CountryCode,Number")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
                user.ModifiedAt = DateTime.UtcNow;
                
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();
                
                TempData["SuccessMessage"] = _localizer != null ? 
                    _localizer["UserCreatedSuccessfully"].Value : 
                    "User has been created successfully";
                    
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,SurName,Email,CountryCode,Number,CreatedAt")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original user to preserve some data
                    var originalUser = await _unitOfWork.Users.GetByIdAsync(id);
                    if (originalUser != null)
                    {
                        // Update only the allowed fields
                        originalUser.FirstName = user.FirstName;
                        originalUser.SurName = user.SurName;
                        originalUser.Email = user.Email;
                        originalUser.CountryCode = user.CountryCode;
                        originalUser.Number = user.Number;
                        originalUser.ModifiedAt = DateTime.UtcNow;
                        
                        await _unitOfWork.Users.UpdateAsync(originalUser);
                        await _unitOfWork.CompleteAsync();
                        
                        TempData["SuccessMessage"] = _localizer != null ? 
                            _localizer["UserUpdatedSuccessfully"].Value : 
                            "User has been updated successfully";
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    if (!await UserExists(user.Id))
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
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                // Soft delete implementation
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
                user.DeletedBy = GetCurrentUserId(); // Implement proper user ID retrieval
                
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();
                
                TempData["SuccessMessage"] = _localizer != null ? 
                    _localizer["UserDeletedSuccessfully"].Value : 
                    "User has been deleted successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null;
        }
        
        protected Guid GetCurrentUserId()
        {
            try
            {
                // Implementation depends on your authentication mechanism
                // For now, return empty GUID as a fallback
                return Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}