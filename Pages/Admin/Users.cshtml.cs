using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbeerRestaurant.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserViewModel> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allUsers = _userManager.Users.ToList();
            var currentUser = await _userManager.GetUserAsync(User);

            Users = allUsers.Select(user => new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = _userManager.IsInRoleAsync(user, "Admin").Result,
                IsCurrentUser = user.Id == currentUser?.Id
            }).ToList();
        }

        public async Task<IActionResult> OnPostPromoteAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDemoteAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            await _userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToPage();
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
