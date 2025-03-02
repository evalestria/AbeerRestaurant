using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersModel(AbeerRestaurantContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<OrderViewModel> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            Orders = orders.Select(order => new OrderViewModel
            {
                ID = order.ID,
                UserEmail = _userManager.FindByIdAsync(order.UserId).Result?.Email ?? "Unknown",
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();
        }
    }

    public class OrderViewModel
    {
        public int ID { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public System.DateTime OrderDate { get; set; }
    }
}
