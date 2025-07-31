namespace HyperBean.Models
{
    class Admin
    {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;

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
            else if (Username.Length > 15)
            {
                error_list.Add("Username should not exceed more than 15 characters");
            }
            else if (Username.Length < 5)
            {
                error_list.Add("Username should not exceed lower than 5 characters");
            }

            // Password length

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

            if (error_list.Any())
            {
                return false;
            }

            return true;
        }
    }
}