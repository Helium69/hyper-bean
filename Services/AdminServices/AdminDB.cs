using Microsoft.Data.Sqlite;
using BCrypt.Net;
using HyperBean.Models;

namespace HyperBean.Services.AdminServices
{
    class AdminDB
    {
        private async Task InitiateDB()
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.AdminDB}"))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.AdminTable} (
                        username NOT NULL,
                        password NOT NULL
                        )
                    ";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ValidateDB(Admin user)
        {
            Admin admin = new Admin();
            await InitiateDB();

            using (var connection = new SqliteConnection($"Data Source={Filename.AdminDB}"))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.AdminTable}";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            admin.Username = reader["username"].ToString();
                            admin.Password = reader["password"].ToString();
                        }
                    }
                }
            }

            if (admin.Username != user.Username || !BCrypt.Net.BCrypt.Verify(user.Password, admin.Password))
            {
                return false;
            }

            return true;
        }

        /*
        ONE TIME USE ONLY TO POPULATE THE ADMIN


        public void QuickInsert()
        {
            InitiateDB();
            string username = "admin";
            string pass = "admin123";

            string password = BCrypt.Net.BCrypt.HashPassword(pass);

            using (var connection = new SqliteConnection($"Data Source={Filename.AdminDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO {Filename.AdminTable} (username, password) VALUES (@username, @password)";

                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    command.ExecuteNonQuery();
                }
            }
        }

        */
    }
}