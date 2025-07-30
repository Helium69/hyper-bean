using System.Text.RegularExpressions;

namespace HyperBean.Models
{
    class Admin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Error List
        private List<string> error_list = new List<string>();
        public List<string> ErrorList
        {
            get { return error_list; }
        }
        public bool IsValuesValid()
        {
            if (Username is null || Password is null)
            {
                error_list.Add("Data possibly corrupted");
                return false;
            }
            
            // Username length 

            if (string.IsNullOrWhiteSpace(Username))
            {
                error_list.Add("Username should not be empty");
            }

            if (Username.Length > 15)
            {
                error_list.Add("Username should not exceed more than 15 characters");
            }

            if (Username.Length < 5)
            {
                error_list.Add("Username should not exceed lower than 5 characters");
            }

            // Password length

            if (string.IsNullOrWhiteSpace(Password))
            {
                error_list.Add("Password should not be empty");
            }

            if (Password.Length > 15)
            {
                error_list.Add("Password should not exceed more than 15 characters");
            }

            if (Password.Length < 5)
            {
                error_list.Add("Password should not exceed lower than 5 characters");
            }

            if (error_list.Any())
            {
                return false;
            }

            return true;
        }
    }
}