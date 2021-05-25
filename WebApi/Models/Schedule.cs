using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class Schedule
    {
        public DateTime Date { get; set; }
        public List<TimeBlock> TimeBlocks { get; set; }

        public Schedule()
        {
            TimeBlocks = new List<TimeBlock>();
        }
    }
}
