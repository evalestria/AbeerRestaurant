using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Pages.Menu
{
    public class ViewModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;

        public ViewModel(AbeerRestaurantContext context)
        {
            _context = context;
        }

        public IList<FoodItem> FoodItem { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            IQueryable<FoodItem> query = _context.FoodItem.Where(f => f.Available);

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(f => f.Item_name.Contains(SearchQuery) || f.Item_desc.Contains(SearchQuery));
            }

            FoodItem = await query.ToListAsync();
        }

        public IActionResult OnPostAddToCart(int ItemId)
        {
            var cart = GetCart();
            var item = _context.FoodItem.Find(ItemId);
            if (item != null)
            {
                if (cart.ContainsKey(ItemId))
                    cart[ItemId]++;
                else
                    cart[ItemId] = 1;
            }
            SaveCart(cart);

            return RedirectToPage();
        }

        private Dictionary<int, int> GetCart()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return cart == null ? new Dictionary<int, int>() : JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
        }

        private void SaveCart(Dictionary<int, int> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
    }
}
