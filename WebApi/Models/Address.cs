namespace WebApi.Models
{
    public class Address
    {
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
    }
}