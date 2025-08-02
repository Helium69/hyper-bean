using HyperBean.Services.AdminServices;
using Microsoft.Data.Sqlite;
using HyperBean.Models;
using System.ComponentModel;

namespace HyperBean.Services
{
    class CoffeeDB
    {
        private void InitiateCoffee()
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.CoffeeTable} (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL UNIQUE,
                        small REAL NOT NULL,
                        medium REAL NOT NULL,
                        large REAL NOT NULL,
                        is_available INTEGER NOT NULL,
                        url TEXT NOT NULL
                        );";

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertCoffee(Coffee coffee)
        {
            InitiateCoffee();

            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        INSERT INTO {Filename.CoffeeTable} (name, small, medium, large, is_available, url)
                        VALUES (@name, @small, @medium, @large, @is_available, @url);
                    ";

                    int converted_bool = coffee.IsAvailable == true ? 1 : 0;

                    command.Parameters.AddWithValue("@name", coffee.Name);
                    command.Parameters.AddWithValue("@small", coffee.Small);
                    command.Parameters.AddWithValue("@medium", coffee.Medium);
                    command.Parameters.AddWithValue("@large", coffee.Large);
                    command.Parameters.AddWithValue("@is_available", converted_bool);
                    command.Parameters.AddWithValue("@url", coffee.URL);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Coffee> GetCoffee()
        {
            List<Coffee> coffee_list = new List<Coffee>();


            InitiateCoffee();
            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.CoffeeTable}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Coffee coffee = new Coffee();
                            coffee.Name = reader["name"].ToString();
                            coffee.Small = Convert.ToDouble(reader["small"]);
                            coffee.Medium = Convert.ToDouble(reader["medium"]);
                            coffee.Large = Convert.ToDouble(reader["large"]);
                            coffee.IsAvailable = Convert.ToInt32(reader["is_available"]) == 1;
                            coffee.URL = reader["url"].ToString();
                            coffee.ID = Convert.ToInt32(reader["id"]);

                            coffee_list.Add(coffee);
                        }
                    }
                }
            }

            return coffee_list;
        }

        public bool UpdateCoffeeAvailability(Coffee coffee)
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        UPDATE {Filename.CoffeeTable}
                        SET is_available = @is_available
                        WHERE name = @name;
                    ";

                    int converted_status = (bool) coffee.IsAvailable! ? 1 : 0;

                    command.Parameters.AddWithValue("@name", coffee.Name);
                    command.Parameters.AddWithValue("@is_available", converted_status);

                    int change_count = command.ExecuteNonQuery();

                    if (change_count != 0) return true;

                    return false;
                }
            }
        }

        public void DeleteCoffee(Coffee coffee)
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM {Filename.CoffeeTable} WHERE name = @name;";
                    command.Parameters.AddWithValue("@name", coffee.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        // THIS PART BELOW IS ONLY ADD ON


        public void InitiateAddon()
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.AddonTable} (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL UNIQUE,
                        price REAL NOT NULL,
                        url TEXT NOT NULL,
                        is_available INTEGER NOT NULL
                        );";

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertAddon(Addon addon)
        {
            InitiateAddon();

            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        INSERT INTO {Filename.AddonTable} (name, url, price, is_available) 
                        VALUES (@name, @url, @price, @is_available)";

                    command.Parameters.AddWithValue("@name", addon.Name);
                    command.Parameters.AddWithValue("@url", addon.URL);
                    command.Parameters.AddWithValue("@price", addon.Price);
                    command.Parameters.AddWithValue("@is_available", addon.IsAvailable);

                    command.ExecuteNonQuery();
                }
            }
        }



        public List<Addon> GetAddon()
        {
            List<Addon> addon_list = new List<Addon>();

            InitiateAddon();

            using (var connection = new SqliteConnection($"Data Source={Filename.CoffeeDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.AddonTable}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Addon addon = new Addon();

                            addon.Name = reader["name"].ToString();
                            addon.Price = Convert.ToDouble(reader["price"]);
                            addon.URL = reader["url"].ToString();
                            addon.ID = Convert.ToInt32(reader["id"]);
                            addon.IsAvailable = Convert.ToInt32(reader["id"]) == 1;

                            addon_list.Add(addon);
                        }
                    }
                }
            }

            return addon_list;
        }

        
    }
}