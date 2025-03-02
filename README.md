# Abeer Restaurant Web App

This is a restaurant web application developed by **Abeer Izar**. It allows customers to browse the menu, add items to the cart, place orders, and view order history. Admin users can manage the menu, view orders, and manage users.

## Database Reset Instructions

To reset the database, run the following commands in the console:

```sh
dotnet ef database drop --context AbeerRestaurantContext
dotnet ef migrations remove --context AbeerRestaurantContext
dotnet ef migrations add InitialCreate --context AbeerRestaurantContext
dotnet ef migrations add AddImageUrlToFoodItem --context AbeerRestaurantContext
dotnet ef database update --context AbeerRestaurantContext
dotnet ef database update --context ApplicationDbContext
```

## Deployment to Chester Network

After deploying to the Chester Network, ensure the web.config file contains the following:

```sh
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified"/>
    </handlers>
    <aspNetCore processPath="dotnet" arguments=".\AbeerRestaurant.dll" stdoutLogEnabled="true" stdoutLogFile="logs\stdout" hostingModel="InProcess"/>
  </system.webServer>
</configuration>
```

Update the appsettings.json file with the Chester Network database connection string:

```sh
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "AbeerRestaurantContext": "Server=mssql.chester.network;Database=db_2325345_co5227;User Id=user_db_2325345_co5227;Password=*********;TrustServerCertificate=True;MultipleActiveResultSets=true;"
  }
}
```

## Features

User Functionality
```sh
✅ View menu items with images.
✅ Add items to cart and checkout.
✅ View order history.
```

Admin Functionality
```sh
✅ Manage menu (add, edit, delete food items).
✅ Upload food item images.
✅ View and manage orders.
✅ Manage registered users.
✅ Promote/Demote users into Admins.
```