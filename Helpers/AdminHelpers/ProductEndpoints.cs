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
    }
}