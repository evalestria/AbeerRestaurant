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
            var adminUser = await userManager.FindByEmailAsync("abeer@abeer.com");

            if (adminUser == null)
            {
                // 3. Create admin user if it doesn't exist
                adminUser = new IdentityUser
                {
                    UserName = "abeer@abeer.com",
                    Email = "abeer@abeer.com",
                    EmailConfirmed = true // Auto-confirm email
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

            var fooditems = new FoodItem[]
            {
                new FoodItem{Item_name="Shepherds Pie",Item_desc="Our tasty shepherds pie packed full of lean minced lamb and an assortment of vegetables",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Cottage Pie",Item_desc="Our tasty cottage pie packed full of lean minced beef and an assortment of vegetables",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Haggis,Neeps and Tatties",Item_desc="Scotland national Haggis dish. Sheep’s heart, liver, and lungs are minced, mixed with suet and oatmeal, then seasoned with onion, cayenne, and our secret spice. Served with boiled turnips and potatoes (‘neeps and tatties’)",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Bangers and Mash",Item_desc="Succulent sausages nestled on a bed of buttery mashed potatoes and drenched in a rich onion gravy",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Toad in the Hole",Item_desc="Ultimate toad-in-the-hole with caramelised onion gravy",Available=true,Vegetarian=false}

            };

            context.FoodItem.AddRange(fooditems);
            context.SaveChanges();
        }
    }
}
