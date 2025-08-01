using HyperBean.Services.AdminServices;
using Microsoft.Data.Sqlite;
using HyperBean.Models;

namespace HyperBean.Services
{
    class CoffeeDB
    {
        private void InitiateDB()
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
            InitiateDB();

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


            InitiateDB();
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
                            coffee.Small = Convert.ToInt32(reader["small"]);
                            coffee.Medium = Convert.ToInt32(reader["medium"]);
                            coffee.Large = Convert.ToInt32(reader["large"]);
                            coffee.IsAvailable = Convert.ToInt32(reader["is_available"]) == 1;
                            coffee.URL = reader["url"].ToString();
                        }
                    }
                }
            }

            return coffee_list;
        }
    }
}