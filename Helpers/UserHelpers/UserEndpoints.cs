using HyperBean.Models;
using HyperBean.Services.UserServices;

namespace HyperBean.Helpers.UserHelpers
{
    class UserEndpoints
    {
        public async Task<IResult> InsertUser(HttpContext context)
        {
            User? user;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                user = await context.Request.ReadFromJsonAsync<User>();

                if (user is null)
                {
                    Console.WriteLine("[DEBUG] data is null or corrupted");

                    response.Message = "Data received is empty";
                    return Results.Json(response, statusCode: 400);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] data serialization failed");

                response.Message = "Data deserialization failed";
                return Results.Json(response, statusCode: 400);
            }

            if (!user.IsValuesValid())
            {
                Console.WriteLine("[DEBUG] Invalid Input");
                ResponseAPI<List<string>> errors = new ResponseAPI<List<string>>();

                errors.Message = "Invalid User Input";
                errors.Data = user.ErrorList;
                return Results.Json(response, statusCode: 422);
            }

            UserDB service = new UserDB();

            if (!service.InsertUser(user))
            {
                Console.WriteLine("[DEBUG] User already Exist");

                response.Message = "User already exist\'s";
                return Results.Json(response, statusCode: 409);
            }

            Console.WriteLine("[DEBUG] Success");

            response.Message = "Success";
            return Results.Json(response, statusCode: 201);
        }
    }
}