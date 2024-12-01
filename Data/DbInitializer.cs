using AbeerRestaurant.Models;
using System.Linq;

namespace AbeerRestaurant.Data
{
    public class DbInitializer
    {
        public static void Initialize(AbeerRestaurantContext context)
        {
            if (context.FoodItem.Any())
            {
                return;
            }

            var fooditems = new FoodItem[]
            {
                new FoodItem{Item_name="Apple Pie", Item_desc="Our tasty Apple Pie packed with cream.", Available=true, Vegetarian=true, Price=9}
            };

            context.FoodItem.AddRange(fooditems);
            context.SaveChanges();
        }
    }
}
