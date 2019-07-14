using CoffeeRunAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.ViewModels
{
    public class RunViewModel
    {
        public int RunId { get; set; }
        public string Name { get; set; }
        public DateTime TimeToRun { get; set; }
        public RunStatus runStatus { get; set; }
        public string Runner { get; set; }
        public string OwnerUserId { get; set; }
    }
}
