namespace HyperBean.Models
{
    class Addon
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public bool? IsAvailable { get; set; }
        public string? URL { get; set; }
        

        private List<string> error_list = new List<string>();
        public List<string> ErrorList
        {
            get { return error_list; }
        }

        public bool IsValuesValid()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                error_list.Add("Name should not be empty");
            }
            else if (Name.Length > 25)
            {
                error_list.Add("Name should not exceed more than 25 characters");
            }
            else if (Name.Length < 2)
            {
                error_list.Add("Name should not exceed lower than 2 characters");
            }

            if (string.IsNullOrWhiteSpace(URL))
            {
                error_list.Add("URL should not be empty");
            }

            if (Price is null)
            {
                error_list.Add("Price should not be empty");
            }

            if (Price > 1000)
            {
                error_list.Add("Add-on price should not exceed more than 1000 pesos");
            }

            if (Price <= 0)
            {
                error_list.Add("Add-on price should not exceed lower or equal than 0 pesos");
            }

            if (IsAvailable is null)
            {
                error_list.Add("Default value for add - on status is required");
            }

            if (error_list.Any()) return false;

            return true;
        }
    }
}