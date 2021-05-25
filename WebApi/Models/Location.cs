using System.Collections.Generic;

namespace WebApi.Models
{
    public class Location
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Address> Addresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }

        public Location()
        {
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
        }
    }

}
