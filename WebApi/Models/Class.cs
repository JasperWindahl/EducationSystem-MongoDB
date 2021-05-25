using System.Collections.Generic;

namespace WebApi.Models
{
    public class Class
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClassCode { get; set; }
        public List<Schedule> Schedules { get; set; }

        public Class()
        {
            Schedules = new List<Schedule>();
        }
    }
}
