using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbeerRestaurant.Pages.Admin
{
    [Authorize(Roles = "Admin")] // Restricts this page to admin users only
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
