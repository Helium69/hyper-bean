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

                using (var pragmaCommand = connection.CreateCommand())
                {
                    pragmaCommand.CommandText = "PRAGMA foreign_keys = ON";
                    pragmaCommand.ExecuteNonQuery();
                }

                using (var commandUser = connection.CreateCommand())
                {
                    commandUser.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.UserTable} (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL UNIQUE,
                        sex TEXT NOT NULL,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL,
                        birth_date TEXT NOT NULL,
                        user_balance REAL NOT NULL,
                        is_active INTEGER NOT NULL,
                        url TEXT NOT NULL);
                    ";

                    commandUser.ExecuteNonQuery();
                }

                using (var commandTransaction = connection.CreateCommand())
                {
                    commandTransaction.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {Filename.TransactionTable} (
                        user_id INTEGER NOT NULL,
                        total_fee REAL NOT NULL,
                        balance_amount_left REAL NOT NULL,
                        date TEXT NOT NULL,

                        FOREIGN KEY (user_id) REFERENCES {Filename.UserTable}(id) ON DELETE CASCADE
                        );
                    ";

                    commandTransaction.ExecuteNonQuery();
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

        public bool ValidateUserLogin(User user_input, out int? id)
        {
            InitiateDB();
            User user_account = new User();

            using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        SELECT password, id FROM {Filename.UserTable}
                        WHERE username = @username
                    ";

                    command.Parameters.AddWithValue("@username", user_input.Username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            id = null;
                            return false;
                        }

                        user_account.Password = reader["password"].ToString();
                        user_account.ID = Convert.ToInt32(reader["id"]);
                    }
                }
            }

            if (!BCrypt.Net.BCrypt.Verify(user_input.Password, user_account.Password))
            {
                id = null;
                return false;
            }

            id = user_account.ID;
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

        public bool UpdateUserStatus(User user)
        {
            InitiateDB();
            using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        UPDATE {Filename.UserTable}
                        SET is_active = @is_active
                        WHERE id = @id;
                    ";

                    command.Parameters.AddWithValue("@is_active", (bool)user.IsActive! ? 0 : 1);
                    command.Parameters.AddWithValue("@id", user.ID);

                    int changes = command.ExecuteNonQuery();

                    if (changes == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public User? GetUserAccount(int id)
        {
            InitiateDB();


            User user = new User();

            using (var connection = new SqliteConnection($"Data Source={Filename.UserDB}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.UserTable} WHERE id = @id";

                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        user.ID = Convert.ToInt32(reader["id"]);
                        user.Name = reader["name"].ToString();
                        user.Username = reader["username"].ToString();
                        user.IsActive = Convert.ToInt32(reader["is_active"]) == 1 ? true : false;
                        user.Sex = reader["sex"].ToString();
                        user.UserBalance = Convert.ToDouble(reader["user_balance"]);
                        user.BirthDate = reader["birth_date"].ToString();
                        user.URL = reader["url"].ToString();
                    }
                }
            }

            return user;
        }

        // Transaction table

        public void InsertTransaction(Transaction transaction)
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.TransactionTable}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        INSERT INTO {Filename.TransactionTable} (user_id, total_fee, balance_amount_left, date)
                        VALUES (@user_id, @total_fee, @balance_amount_left , @date);
                    ";

                    command.Parameters.AddWithValue("@user_id", transaction.Date);
                    command.Parameters.AddWithValue("@total_fee", transaction.TotalFee);
                    command.Parameters.AddWithValue("@balance_amount_left", transaction.BalanceAmountLeft);
                    command.Parameters.AddWithValue("@date", transaction.Date);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Transaction> GetTransactions(int id)
        {
            List<Transaction> transaction_list = new List<Transaction>();
            using (var connection = new SqliteConnection($"Data Source={Filename.TransactionTable}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Filename.TransactionTable} WHERE user_id = @user_id";

                    command.Parameters.AddWithValue("@user_id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Transaction transaction = new Transaction();
                            transaction.ID = Convert.ToInt32(reader["user_id"]);
                            transaction.BalanceAmountLeft = Convert.ToDouble(reader["balance_amount_left"]);
                            transaction.TotalFee = Convert.ToDouble(reader["total_fee"]);
                            transaction.Date = reader["date"].ToString()!;

                            transaction_list.Add(transaction);
                        }
                    }
                }
            }

            return transaction_list;
        }

    }
}