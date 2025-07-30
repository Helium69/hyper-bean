namespace HyperBean.Models
{
    class ResponseAPI<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}