using AbeerRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AbeerRestaurant.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(AbeerRestaurantContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // 1. Ensure the Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Check if the admin user exists
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

            if (adminUser == null)
            {
                // 3. Create admin user if it doesn't exist
                adminUser = new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "123456");

                if (result.Succeeded)
                {
                    // 4. Assign the "Admin" role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                // 5. Ensure the user is in the Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }



            // 6. Add food items if the database is empty
            if (context.FoodItem.Any())
            {
                return;
            }

            // 7. Set initial delicious stuff
            var fooditems = new FoodItem[]
            {
                new FoodItem{Item_name="Basil & Mascarpone Chicken",
                    Item_desc="Higher-welfare British diced chicken breast marinated with lemon and garlic in a basil pesto and mascarpone sauce, with semi-dried cherry tomatoes.",
                    Available=true,Vegetarian=false, Price=10, ImageUrl="product_1606_4142.jpg"},
                new FoodItem{Item_name="Honey & Ginger Chicken",
                    Item_desc="Soy-marinated higher-welfare British chicken in a honey, ginger and garlic sauce with sesame-topped choi sum, yellow peppers and red onions.",
                    Available=true,Vegetarian=false, Price=5, ImageUrl="product_2060_4348.png"},
                new FoodItem{Item_name="Garlic Chicken Curry",
                    Item_desc="A classic garlic, coriander, onion and tomato sauce with marinated higher-welfare British chicken breast. ",
                    Available=true,Vegetarian=false, Price=6, ImageUrl="product_407_4338.png"},
                new FoodItem{Item_name="Veggie Cottage Pie",
                    Item_desc="Quorn mince cooked in a rich, tomato, red wine and thyme sauce, topped with buttery mash, cheese and parsley.",
                    Available=true,Vegetarian=true, Price=8, ImageUrl="product_2245_5935.jpg"},
                new FoodItem{Item_name="Halloumi & Arrabbiata Pasta Bake",
                    Item_desc="Roasted courgettes, aubergines and halloumi with cannolicchi pasta in a spicy tomato sauce, topped with crumbled feta.",
                    Available=true,Vegetarian=true, Price=7, ImageUrl="product_1958_6269.jpg"},
                new FoodItem{Item_name="Teriyaki Salmon Noodles",
                    Item_desc="Salmon fillet with egg noodles, tenderstem broccoli, mangetout and baby corn in our teriyaki sauce topped with sesame seeds.",
                    Available=true,Vegetarian=false, Price=9, ImageUrl="product_2428_6109.jpg"}
            };

            // 8. Start cooking!
            context.FoodItem.AddRange(fooditems);
            context.SaveChanges();
        }
    }
}
