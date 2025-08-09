using HyperBean.Models;
using HyperBean.Services;
using HyperBean.Services.UserServices;

namespace HyperBean.Helpers.UserHelpers
{
    class UserEndpoints
    {
        record Payment(double payment);

        public IResult GetCurrentUser(HttpContext context)
        {

            User user;

            int? id = context.Session.GetInt32("UserID");

            if (id is null)
            {
                ResponseAPI<string> error = new ResponseAPI<string>();
                error.Message = "Unauthorized Access Detected";
                return Results.Json(error, statusCode: 401);
            }

            UserDB service = new UserDB();

            user = service.GetUserAccount((int)id)!;

            ResponseAPI<User> response = new ResponseAPI<User>();

            response.Message = "Success";
            response.Data = user;

            return Results.Json(response, statusCode: 200);
        }

        public async Task<IResult> AddFunds(HttpContext context)
        {
            Payment? payment; // payment
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                payment = await context.Request.ReadFromJsonAsync<Payment>();

                if (payment is null)
                {
                    Console.WriteLine("[DEBUG] payment null");
                    response.Message = "Data is null, data might be corrupted";
                    return Results.Json(response, statusCode: 400);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] deserialization failed");
                response.Message = "Data serialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (payment.payment < 1)
            {
                Console.WriteLine("[DEBUG] invalid payment");
                response.Message = "Invalid User Input";
                return Results.Json(response, statusCode: 422);
            }

            UserDB service = new UserDB();
            int? userID = context.Session.GetInt32("UserID"); // id

            if (userID is null)
            {
                Console.WriteLine("[DEBUG] unauthorized");
                return Results.Json(response, statusCode: 401);
            }

            User? currentUser = service.GetUserAccount((int)userID); // user

            if (currentUser is null)
            {
                Console.WriteLine("[DEBUG] no user found");
                response.Message = "No user account was found";
                return Results.Json(response, statusCode: 401);
            }

            double newBalance = payment.payment + (double)currentUser.UserBalance!;

            if (newBalance > 10000)
            {
                Console.WriteLine("+ limit");
                response.Message = "Adding funds exceed the ₱10000 limit";
                return Results.Json(response, statusCode: 422);
            }

            service.UpdateFunds((int)userID, newBalance);

            ResponseAPI<double> ok = new ResponseAPI<double>();
            ok.Message = "Success";
            ok.Data = newBalance;
            
            return Results.Json(ok, statusCode: 200);
        }

        public async Task<IResult> BuyCoffee(HttpContext context)
        {
            Order? order; // Order
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                order = await context.Request.ReadFromJsonAsync<Order>();

                if (order is null || !order.IsValuesValid)
                {
                    Console.WriteLine("[DEBUG] data null");

                    response.Message = "Null detected in required data field\'s, data might be corrupted";
                    return Results.Json(response, statusCode: 400);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] data deserialization failed");
                response.Message = "Data serialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (order.Quantity < 1)
            {
                Console.WriteLine("[DEBUG] q below");
                response.Message = "Invalid quantity detected";
                return Results.Json(response, statusCode: 422);
            }

            if (order.Quantity > 5)
            {
                Console.WriteLine("[DEBUG] q upper");
                response.Message = "Maximum quantity reached";
                return Results.Json(response, statusCode: 422);
            }

            int? userID = context.Session.GetInt32("UserID");

            if (userID is null)
            {
                Console.WriteLine("[DEBUG] unauthorized");
                response.Message = "Unauthorized access detected";
                return Results.Json(response, statusCode: 401);
            }

            UserDB service = new UserDB();

            User? user = service.GetUserAccount((int)userID); // user

            if (user is null)
            {
                Console.WriteLine("[DEBUG] user null");
                response.Message = "Does not exist";
                return Results.Json(response, statusCode: 401);
            }

            CoffeeDB productService = new CoffeeDB();

            double coffeePrice = (double)productService.GetCoffeePrice((int)order.CoffeeID!, order.CoffeeSize!)!;

            ResponseAPI<double> ok = new ResponseAPI<double>();


            if (order.AddonsID is not null)
            {
                double addonTotalPrice = productService.GetAddonTotalPrice(order.AddonsID);

                double totalPrice = (coffeePrice + addonTotalPrice) * Convert.ToDouble(order.Quantity);

                double newUserBalance = (double)user.UserBalance! - totalPrice;

                if (newUserBalance < -10000)
                {
                    Console.WriteLine("[DEBUG] - limit");
                    response.Message = "Maximum debt should not reach ₱-10000";
                    return Results.Json(response, statusCode: 422);
                }

                Console.WriteLine("[DEBUG] success");

                service.UpdateFunds((int)userID, newUserBalance);

                ok.Message = "Order Purchased!";
                ok.Data = totalPrice;
                return Results.Json(ok, statusCode: 200);
            }

            double totalCoffeePrice = coffeePrice * Convert.ToDouble(order.Quantity);

            double userBalance = (double)user.UserBalance! - totalCoffeePrice;



            if (userBalance < -10000)
            {
                Console.WriteLine("[DEBUG] - limit 2");
                response.Message = "Maximum debt should not reach ₱-10000";
                return Results.Json(response, statusCode: 422);
            }

            service.UpdateFunds((int)userID, userBalance);

            Console.WriteLine("[DEBUG] - success 2 ");

            ok.Message = "Order Purchased!";
            ok.Data = totalCoffeePrice;
            return Results.Json(ok, statusCode: 200);

        }

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

                return Results.Json(errors, statusCode: 422);
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

        public IResult GetUsers()
        {
            UserDB service = new UserDB();

            List<User> user_list;

            try
            {
                user_list = service.GetUsers();
            }
            catch (Exception)
            {
                ResponseAPI<string> error = new ResponseAPI<string>();
                error.Message = "Something went wrong from the db server";
                return Results.Json(error, statusCode: 500);
            }

            ResponseAPI<List<User>> response = new ResponseAPI<List<User>>();
            response.Message = "Success";
            response.Data = user_list;
            return Results.Json(response, statusCode: 200);
        }
        public async Task<IResult> UpdateStatus(HttpContext context)
        {
            User? user;
            ResponseAPI<string> response = new ResponseAPI<string>();

            try
            {
                user = await context.Request.ReadFromJsonAsync<User>();

                if (user is null || user.ID is null)
                {
                    Console.WriteLine("[DEBUG] data is null");
                    response.Message = "Data received is null, data might be corrupted";
                    return Results.Json(response, statusCode: 400);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] data serialization failed");
                response.Message = "Data serialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            //find if it exist first and use that instead of this one

            UserDB service = new UserDB();

            User? obtained_user;
            try
            {
                obtained_user = service.GetUserAccount((int)user.ID);

                if (obtained_user is null)
                {
                    Console.WriteLine("[DEBUG] user not found");
                    response.Message = "User not found";
                    return Results.Json(response, statusCode: 500);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] something went wrong from the server");
                response.Message = "Something went wrong from the server";
                return Results.Json(response, statusCode: 500);
            }



            if (!service.UpdateUserStatus(obtained_user))
            {
                Console.WriteLine("[DEBUG] data failed to update");
                response.Message = "Failed to update user data";
                return Results.Json(response, statusCode: 500);
            }

            response.Message = "Success";
            return Results.Json(response, statusCode: 200);


        }

        public IResult ValidateUserSession(HttpContext context)
        {
            ResponseAPI<string> response = new ResponseAPI<string>();

            if (context.Session.GetInt32("UserID") is null)
            {
                Console.WriteLine("[DEBUG] unauthorized");
                response.Message = "Unauthorized access detected";
                return Results.Json(response, statusCode: 401);
            }

            response.Message = "Success";
            return Results.Json(response, statusCode: 200);
        }

        public async Task<IResult> ValidateUserLogin(HttpContext context)
        {
            ResponseAPI<string> response = new ResponseAPI<string>();
            User? user_input;

            try
            {
                user_input = await context.Request.ReadFromJsonAsync<User>();
            }
            catch (Exception)
            {
                Console.WriteLine("[DEBUG] deserialization failed");
                response.Message = "Data deserialization failed, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            if (user_input is null)
            {
                Console.WriteLine("[DEBUG] data is null");
                response.Message = "Data is null, data might be corrupted";
                return Results.Json(response, statusCode: 400);
            }

            UserDB service = new UserDB();

            int? user_id;

            if (!service.ValidateUserLogin(user_input, out user_id))
            {
                Console.WriteLine("[DEBUG] login failed");
                response.Message = "Invalid username or password";
                return Results.Json(response, statusCode: 401);
            }

            context.Session.Clear();
            context.Session.SetInt32("UserID", (int)user_id!);

            Console.WriteLine("[DEBUG] success");

            response.Message = "Success";
            return Results.Json(response, statusCode: 200);
        }

    }   
}