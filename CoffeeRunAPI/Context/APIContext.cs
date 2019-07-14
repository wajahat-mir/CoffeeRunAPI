using CoffeeRunAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.Context
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Run> Runs { get; set; }
    }
}
