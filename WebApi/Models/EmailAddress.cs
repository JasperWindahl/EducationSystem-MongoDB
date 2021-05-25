namespace WebApi.Models
{
    public class EmailAddress
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
    }
}