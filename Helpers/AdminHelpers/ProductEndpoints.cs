using HyperBean.Models;
using HyperBean.Services;
using Microsoft.Data.Sqlite;

namespace HyperBean.Helpers.AdminHelpers
{
    class ProductEndpoints
    {
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
    }
}