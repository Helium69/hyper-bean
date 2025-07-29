using System.Text.RegularExpressions;

namespace HyperBean.Models
{
    class Admin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Error List
        private List<string>? error_list;
        public List<string>? ErrorList()
        {
            if (Username is null || Password is null)
            {
                error_list?.Add("Data possibly corrupted");
                return error_list;
            }

            // Username length 
            if (Username.Length > 15)
            {
                error_list!.Add("Username should not exceed more than 15 characters");
            }

            if (Username.Length < 5)
            {
                error_list!.Add("Username should not exceed lower than 5 characters");
            }

            // Password length

            if (Password.Length > 15)
            {
                error_list!.Add("Password should not exceed more than 15 characters");
            }

            if (Password.Length < 5)
            {
                error_list!.Add("Password should not exceed lower than 5 characters");
            }

            return error_list;
        }
    }
}