using System.Collections.Generic;

namespace BackEnd.Models
{
    public class Operation
    {
        public User User { get; set; }
        public int userId { get; set; }
        public Job Job { get; set; }
        public string JobNumber { get; set; }
        public string OpNumber { get; set; }
        public string Machine { get; set; }
        public string RemainingQuantity { get; set; }
        public string CycleTime { get; set; }
        public string PartsToDate { get; set; }
        public ICollection<Hourly> Hourly { get; set; }
        public ICollection<Production> Production { get; set; }
        
    }
}