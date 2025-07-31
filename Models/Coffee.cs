namespace HyperBean.Models
{
    class Coffee
    {
        // Coffee fields
        public string? Name { get; set; }
        public double? Small { get; set; }
        public double? Medium { get; set; }
        public double? Large { get; set; }
        public bool? IsAvailable { get; set; }

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
            // check if a value is null
            if (Name is null || Small is null || Medium is null || Large is null || IsAvailable is null)
            {
                error_list.Add("Null value/s detected, data might be corrupted");
                return false;
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
            if (Small > 1000)
            {
                error_list.Add("Small size price should not exceed more than 1000 pesos");
            }
            else if (Small <= 0)
            {
                error_list.Add("Small size price should not exceed lower or equal than 0 pesos");
            }

            // check medium price
            if (Medium > 1000)
            {

            }
            else if (Medium <= 0)
            {

            }

            // check large price
            if (Large > 1000)
            {

            }
            else if (Large <= 0)
            {

            }

            // price comparison - small -> medium -> large

            if (Small > Medium || Small > Large)
            {

            }

            if (Medium > Large)
            {

            }

            if (error_list.Any()) return false;

            return true;
        }
    }   
}