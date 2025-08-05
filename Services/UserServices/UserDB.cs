using Microsoft.Data.Sqlite;
using HyperBean.Models;
using BCrypt.Net;

namespace HyperBean.Services.UserServices
{
    class UserDB
    {
        public void InitiateDB()
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.UserTable} (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL UNIQUE,
                        sex TEXT NOT NULL,
                        username TEXT NOT NULL,
                        password TEXT NOT NULL,
                        birth_date TEXT NOT NULL,
                        user_balance REAL NOT NULL,
                        is_active INTEGER NOT NULL,
                        url TEXT NOT NULL);
                    ";

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool InsertUser(User user)
        {
            InitiateDB();

            try
            {
                using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $@"
                            INSERT INTO {Filename.UserTable} (name, username, password, sex, birth_date, user_balance, is_active, url)
                            VALUES (@name, @username, @password, @sex, @birth_date, @user_balance, @is_active, @url)
                        ";

                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(user.Password));
                        command.Parameters.AddWithValue("@sex", user.Sex);
                        command.Parameters.AddWithValue("@birth_date", user.BirthDate);
                        command.Parameters.AddWithValue("@user_balance", user.UserBalance);
                        command.Parameters.AddWithValue("@is_active", (bool)user.IsActive! ? 1 : 0);
                        command.Parameters.AddWithValue("@url", user.URL);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                return false;
            }

            return true;
        }

        public List<User> GetUsers()
        {
            List<User> user_list = new List<User>();

            InitiateDB();

            using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.UserTable}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();

                            user.ID = Convert.ToInt32(reader["id"]);
                            user.Name = reader["name"].ToString();
                            user.Username = reader["username"].ToString();
                            user.Sex = reader["sex"].ToString();
                            user.BirthDate = reader["birth_date"].ToString();
                            user.URL = reader["url"].ToString();
                            user.UserBalance = Convert.ToDouble(reader["user_balance"]);
                            user.IsActive = Convert.ToInt32(reader["is_active"]) == 1 ? true : false;

                            user_list.Add(user);
                        }
                    }
                }
            }

            return user_list;
        }
    }
}