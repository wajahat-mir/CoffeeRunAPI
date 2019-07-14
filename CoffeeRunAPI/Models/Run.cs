using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.Models
{
    public class Run
    {
        [Required]
        public int RunId { get; set; }
        public string Name { get; set; }
        public DateTime TimeToRun { get; set; }
        public string Runner { get; set; }
        [EnumDataType(typeof(RunStatus))]
        public RunStatus runStatus { get; set; }
        public string OwnerUserId { get; set; }
    }

    public enum RunStatus
    {
        NoRunner,
        Expired,
        Death,
        Prepped,
        OnTheRun,
        Delayed,
        Complete
    }
}
