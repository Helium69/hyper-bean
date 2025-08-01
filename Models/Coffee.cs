namespace HyperBean.Models
{
    class Coffee
    {
        // Coffee fields
        public int ID { get; set; }
        public string? Name { get; set; }
        public double? Small { get; set; }
        public double? Medium { get; set; }
        public double? Large { get; set; }
        public bool? IsAvailable { get; set; }
        public string? URL { get; set; }

        // Error List

        private List<string> error_list = new List<string>();
        public List<string> ErrorList
        {
            get { return error_list; }
        }

        // Check if the values are valid, please dont forget to use this first before
        // using the error list so potential errors are added to it

        public bool IsValuesValid()
        {
            if (IsAvailable is null)
            {
                error_list.Add("Default value for coffee status is required");
            }

            if (string.IsNullOrWhiteSpace(URL))
            {
                error_list.Add("URL image link should not be empty");
            }
            // check values if valid and within expected values only
            // check name


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


            // check small price
            if (Small is null) {
                error_list.Add("Small size should not be empty");
            }
            else if (Small > 1000)
            {
                error_list.Add("Small size price should not exceed more than 1000 pesos");
            }
            else if (Small <= 0)
            {
                error_list.Add("Small size price should not exceed lower or equal than 0 pesos");
            }

            // check medium price
            if (Medium is null)
            {
                error_list.Add("Medium size should not be empty");
            }
            else if (Medium > 1000)
            {
                error_list.Add("Medium size price should not exceed more than 1000 pesos");
            }
            else if (Medium <= 0)
            {
                error_list.Add("Medium size price should not exceed lower or equal than 0 pesos");
            }

            // check large price
            if (Large is null)
            {
                error_list.Add("Large size should not be empty");
            }
            else if (Large > 1000)
            {
                error_list.Add("Large size price should not exceed more than 1000 pesos");
            }
            else if (Large <= 0)
            {
                error_list.Add("Large size price should not exceed lower or equal than 0 pesos");
            }

            // price comparison - small -> medium -> large

            if (Small > Medium || Small > Large)
            {
                error_list.Add("Small size price should not exceed medium or large size prices");
            }

            if (Medium > Large)
            {
                error_list.Add("Medium size price should not exceed larger size price");
            }

            if (error_list.Any()) return false;

            return true;
        }
    }   
}