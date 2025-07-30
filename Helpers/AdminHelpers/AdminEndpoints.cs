using HyperBean.Models;
using HyperBean.Services.AdminServices;

namespace HyperBean.Helpers.AdminHelpers
{
    class AdminEndpoints
    {
        public async Task<IResult> ValidateLogin(HttpContext context)
        {
            Admin? user;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                user = await context.Request.ReadFromJsonAsync<Admin>();
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG - AdminEndpoint] - Convertion failed, data might be corrupted");

                response.Message = "Data convertion failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (user is null)
            {
                Console.WriteLine("[DEBUG - AdminEndpoint] - Data might be corrupted");

                response.Message = "Data convertion failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (!user.IsValuesValid())
            {
                Console.WriteLine("[DEBUG - AdminEndpoint] - Invalid User Input");

                ResponseAPI<List<string>> errors = new ResponseAPI<List<string>>();
                errors.Data = user.ErrorList;
                errors.Message = "Invalid User Input";

                Console.WriteLine(errors.Data.Count());

                return Results.Json(errors, statusCode: 422);
            }

            AdminDB admin_service = new AdminDB();

            if (!await admin_service.ValidateDB(user))
            {
                Console.WriteLine("[DEBUG - AdminEndpoint] - Unauthorized");
                response.Message = "Unauthorized SignIn, Wrong Username or Password";

                return Results.Json(response, statusCode: 401);
            }

            Console.WriteLine("[DEBUG - AdminEndpoint] - Success");
            response.Message = "Sign in Authorized";

            return Results.Json(response, statusCode: 200);

        }
    }
}