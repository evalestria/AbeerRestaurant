using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AbeerRestaurant.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public class LoginInputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if Email or Password is Empty
            if (string.IsNullOrWhiteSpace(Input.Email) || string.IsNullOrWhiteSpace(Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and password cannot be empty.");
                return Page();
            }

            // Find the user in the database
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No account found with this email.");
                return Page();
            }

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            // Check for wrong password
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Too many failed login attempts. Try again later.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect password. Please try again.");
            }

            return Page();
        }
    }
}
