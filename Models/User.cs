using System.Text.RegularExpressions;

namespace HyperBean.Models
{
    class User
    {
        private string? name;
        private string? sex;

        public int? ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name
        {
            get { return name; }
            set
            {
                name = value?.ToUpper().Trim();
            }
        }

        public string? Sex
        {
            get { return sex; }
            set { sex = value?.Trim().ToUpper(); }
        }

        public string? BirthDate { get; set; }
        public bool? IsActive { get; set; }
        public double? UserBalance { get; set; }
        public string? URL { get; set; }

        private List<string> error_list = new List<string>();
        public List<string> ErrorList {get { return error_list; }}

        public bool IsValuesValid()
        {
            if (Username is null || Password is null || Name is null || Sex is null ||
            BirthDate is null || IsActive is null || UserBalance is null || URL is null)
            {
                error_list.Add("Null detected, data might be corrupted");
                return false;
            }

            // USERNAME
            if (string.IsNullOrWhiteSpace(Username))
            {
                error_list.Add("Username should not be empty");
            }
            else if (Username.Length > 15)
            {
                error_list.Add("Username should not exceed more than 15 characters");
            }
            else if (Username.Length < 5)
            {
                error_list.Add("Username should not exceed lower than 5 characters");
            }

            // PASSWORD

            if (string.IsNullOrWhiteSpace(Password))
            {
                error_list.Add("Password should not be empty");
            }
            else if (Password.Length > 15)
            {
                error_list.Add("Password should not exceed more than 15 characters");
            }
            else if (Password.Length < 5)
            {
                error_list.Add("Password should not exceed lower than 5 characters");
            }

            // NAME -- USE REGEX IN HERE SINCE I FORGOT THE SYNTAX
            Regex regex = new Regex("^[a-zA-Z0-9 ]+$");

            if (string.IsNullOrWhiteSpace(Name))
            {
                error_list.Add("Name should not be empty");
            }
            else if (Name.Length > 60)
            {
                error_list.Add("Name should not exceed more than 60 characters");
            }
            else if (Name.Length < 5)
            {
                error_list.Add("Name should not exceed lower than 5 characters");
            }
            else if (!regex.IsMatch(Name))
            {
                error_list.Add("Name should only contain letters and numbers");
            }

            // SEX

            if (Sex != "MALE" && Sex != "FEMALE")
            {
                error_list.Add("Sex should only have a male or female value");
            }

            // Birth_Date 15+ only
            try
            {
                DateOnly birth_date = DateOnly.Parse(BirthDate);
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                int age = today.Year - birth_date.Year;

                if (today < new DateOnly(today.Year, birth_date.Month, birth_date.Day))
                {
                    age--;
                }

                if (age < 15) {
                    error_list.Add("User should be 15+ years old to have an account");
                }
            }
            catch (Exception)
            {
                error_list.Add("Invalid Date Format");
            }

            // Total Balance
            if (UserBalance > 10000)
            {
                error_list.Add("Balance should not");
            }
            else if (UserBalance < -10000)
            {
                error_list.Add("Balance should not exceed more");
            }

            if (error_list.Any()) return false;

            return true;

        }




    }
}