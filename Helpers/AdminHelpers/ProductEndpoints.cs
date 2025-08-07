using HyperBean.Models;
using HyperBean.Services;
using Microsoft.Data.Sqlite;

namespace HyperBean.Helpers.AdminHelpers
{
    class ProductEndpoints
    {
        // i know i know, the task<result> might have been useless already because i did not async those db services
        // but i think i rather finish this project first since vacation is almost over and i would take note to not forgot that in
        // other projects, and maybe make sure to master topics that relates to async because 1 day of learning it was definitely
        // enough


        public async Task<IResult> InsertCoffee(HttpContext context)
        {
            ResponseAPI<string> response = new ResponseAPI<string>();
            Coffee? coffee_input;

            try
            {
                coffee_input = await context.Request.ReadFromJsonAsync<Coffee>();
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] data might be corrupted");

                response.Message = "Deserialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (coffee_input is null)
            {
                Console.WriteLine("[DEBUG] data might be corrupted");

                response.Message = "Data is null, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (!coffee_input.IsValuesValid())
            {
                Console.WriteLine("[DEBUG] invalid user input");

                ResponseAPI<List<string>> errors = new ResponseAPI<List<string>>();

                errors.Message = "Invalid user input";
                errors.Data = coffee_input.ErrorList;

                return Results.Json(errors, statusCode: 422);
            }

            CoffeeDB service = new CoffeeDB();

            try
            {
                service.InsertCoffee(coffee_input);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                Console.WriteLine("[DEBUG] coffee name already exist\'s");

                response.Message = "Coffee name already exist\'s";
                return Results.Json(response, statusCode: 409);
            }

            Console.WriteLine("[DEBUG] success");

            response.Message = "Coffee has been successfully saved";
            return Results.Json(response, statusCode: 201);
        }

        public async Task<IResult> InsertAddOn(HttpContext context)
        {
            ResponseAPI<string> response = new ResponseAPI<string>();
            Addon? addon_input;

            try
            {
                addon_input = await context.Request.ReadFromJsonAsync<Addon>();

                if (addon_input is null)
                {
                    Console.WriteLine("[DEBUG] whole data is null, might be corrupted");

                    response.Message = "Received data is null, data might be corrupted";
                    return Results.Json(response, statusCode: 400);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] deserialization failed, might be corrupted");

                response.Message = "Data serialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (!addon_input.IsValuesValid())
            {
                Console.WriteLine("[DEBUG] invalid input");

                ResponseAPI<List<string>> error = new ResponseAPI<List<string>>();
                error.Message = "Invalid Input";
                error.Data = addon_input.ErrorList;

                return Results.Json(error, statusCode: 422);
            }

            CoffeeDB service = new CoffeeDB();

            try
            {
                service.InsertAddon(addon_input);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                response.Message = "Add-On Already Exist\'s";
                return Results.Json(response, statusCode: 409);
            }

            response.Message = "Success";
            return Results.Json(response, statusCode: 201);

        }

        public IResult GetAddon()
        {
            List<Addon> result;
            ResponseAPI<string> response = new ResponseAPI<string>();

            CoffeeDB service = new CoffeeDB();

            try
            {
                result = service.GetAddon();
            }
            catch (Exception)
            {
                response.Message = "Data convertion problem, something went wrong from db";
                return Results.Json(response, statusCode: 500);
            }

            if (result is null || result.Count() == 0)
            {
                response.Message = "No data has been saved to database yet";
                return Results.Json(response, statusCode: 200);
            }

            ResponseAPI<List<Addon>> data = new ResponseAPI<List<Addon>>();
            data.Message = "Success";
            data.Data = result;

            return Results.Json(data, statusCode: 200);
        }

        public IResult GetCoffee()
        {
            List<Coffee> result;
            ResponseAPI<string> response = new ResponseAPI<string>();

            CoffeeDB service = new CoffeeDB();

            try
            {
                result = service.GetCoffee();
            }
            catch (Exception)
            {
                response.Message = "Data convertion problem, something went wrong from db";
                return Results.Json(response, statusCode: 500);
            }

            if (result is null || result.Count() == 0)
            {
                response.Message = "No data has been saved to database yet";
                return Results.Json(response, statusCode: 200);
            }

            ResponseAPI<List<Coffee>> data = new ResponseAPI<List<Coffee>>();
            data.Message = "Success";
            data.Data = result;

            return Results.Json(data, statusCode: 200);

        }

        public async Task<IResult> DeleteAddon(HttpContext context)
        {
            Addon? addon;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                addon = await context.Request.ReadFromJsonAsync<Addon>();

            }
            catch (Exception)
            {
                response.Message = "Data deserialization failed, data might be corrupted";

                Console.WriteLine("[DEBUG] data might be corrupted");
                return Results.Json(response.Message, statusCode: 400);
            }

            if (addon is null)
            {
                response.Message = "Data deserialization failed, data might be corrupted";

                Console.WriteLine("[DEBUG] value is null might be corrupted");
                return Results.Json(response.Message, statusCode: 400);
            }

            CoffeeDB service = new CoffeeDB();

            if (!service.DeleteAddon(addon))
            {
                Console.WriteLine("[DEBUG] failed to delete");
                response.Message = "Failed to delete";
                return Results.Json(response.Message, statusCode: 500);
            }

            Console.WriteLine("[DEBUG] success");
            response.Message = "Success";
            return Results.Json(response.Message, statusCode: 200);
        }

        public async Task<IResult> DeleteCoffee(HttpContext context)
        {
            Coffee? coffee;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                coffee = await context.Request.ReadFromJsonAsync<Coffee>();

            }
            catch (Exception)
            {
                response.Message = "Data deserialization failed, data might be corrupted";

                Console.WriteLine("[DEBUG] data might be corrupted");
                return Results.Json(response.Message, statusCode: 400);
            }

            if (coffee is null)
            {
                response.Message = "Data deserialization failed, data might be corrupted";

                Console.WriteLine("[DEBUG] value is null might be corrupted");
                return Results.Json(response.Message, statusCode: 400);
            }

            CoffeeDB service = new CoffeeDB();

            if (!service.DeleteCoffee(coffee))
            {
                Console.WriteLine("[DEBUG] failed to delete");
                response.Message = "Failed to delete";
                return Results.Json(response.Message, statusCode: 500);
            }

            Console.WriteLine("[DEBUG] success");
            response.Message = "Success";
            return Results.Json(response.Message, statusCode: 200);
        }

        public async Task<IResult> UpdateCoffeeStatus(HttpContext context)
        {
            Coffee? coffee;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                coffee = await context.Request.ReadFromJsonAsync<Coffee>();
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] corrupt 1");
                response.Message = "Data deserialization failed, data might be corrupted";
                return Results.Json(response.Message, statusCode: 400);
            }

            if (coffee is null)
            {
                Console.WriteLine("[DEBUG] corrupt 2");
                response.Message = "Data deserialization failed, data might be corrupted";
                return Results.Json(response.Message, statusCode: 400);
            }

            Console.WriteLine($"[DEBUG] Incoming Addon: Name = {coffee.Name}, IsAvailable = {coffee.IsAvailable}");
            CoffeeDB service = new CoffeeDB();

            if (!service.UpdateCoffee(coffee))
            {
                Console.WriteLine("[DEBUG] db failed");
                response.Message = "Failed to update";
                return Results.Json(response.Message, statusCode: 500);
            }


            Console.WriteLine("[DEBUG] success");
            response.Message = "Success";
            return Results.Json(response.Message, statusCode: 200);
        }

        public async Task<IResult> UpdateAddonStatus(HttpContext context)
        {
            Addon? addon;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                addon = await context.Request.ReadFromJsonAsync<Addon>();
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] corrupt 1");
                response.Message = "Data deserialization failed, data might be corrupted";
                return Results.Json(response.Message, statusCode: 400);
            }

            if (addon is null)
            {
                Console.WriteLine("[DEBUG] corrupt 2");
                response.Message = "Data deserialization failed, data might be corrupted";
                return Results.Json(response.Message, statusCode: 400);
            }

            Console.WriteLine($"[DEBUG] Incoming Addon: Name = {addon.Name}, IsAvailable = {addon.IsAvailable}");


            CoffeeDB service = new CoffeeDB();

            if (!service.UpdateAddon(addon))
            {
                Console.WriteLine("[DEBUG] failed db");
                response.Message = "Failed to update";
                return Results.Json(response.Message, statusCode: 500);
            }


            Console.WriteLine("[DEBUG] success for update");
            response.Message = "success";
            return Results.Json(response.Message, statusCode: 200);
        }

        public IResult GetAvailableCoffee()
        {
            CoffeeDB service = new CoffeeDB();
            List<Coffee> availableCoffees;


            try
            {
                availableCoffees = service.GetAvailableCoffee();
            }
            catch (Exception)
            {
                ResponseAPI<string> error = new ResponseAPI<string>();

                error.Message = "Something went wrong from the server\'s database";
                return Results.Json(error, statusCode: 500);
            }

            ResponseAPI<List<Coffee>> response = new ResponseAPI<List<Coffee>>();
            response.Message = "Success";
            response.Data = availableCoffees;

            return Results.Json(response, statusCode: 200);
        }

        public IResult GetAvailableAddOn()
        {
            CoffeeDB service = new CoffeeDB();
            List<Addon> availableAddon;

            try
            {
                availableAddon = service.GetAvailableAddon();
            }
            catch (Exception)
            {
                ResponseAPI<string> error = new ResponseAPI<string>();

                error.Message = "Something went wrong from the server\'s database";
                return Results.Json(error, statusCode: 500);
            }

            ResponseAPI<List<Addon>> response = new ResponseAPI<List<Addon>>();
            response.Message = "Success";
            response.Data = availableAddon;

            return Results.Json(response, statusCode: 200);
        }
    }
}