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
    }
}