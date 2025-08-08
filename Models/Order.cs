namespace HyperBean.Models
{
    class Order
    {
        private string? coffee_size;
        public int? CoffeeID { get; set; }
        public string? CoffeeSize
        {
            get { return coffee_size; }
            set
            {
                coffee_size = value?.Trim().ToLower();
            }
        }
        public int? Quantity { get; set; }
        public List<int>? AddonsID { get; set; }

        public bool IsValuesValid
        {
            get
            {
                if (CoffeeSize is null || Quantity is null || CoffeeID is null) return false;
                return true;
            }
        }
        
    }
}