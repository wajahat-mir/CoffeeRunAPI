using CoffeeRunAPI.Context;
using CoffeeRunAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.Repositories
{
    public interface IRunRepository
    {
        Task<IEnumerable<Run>> GetAllRuns();
        Task<Run> GetRun(int id);
        Task<bool> UpdateRun(Run run);
    }
    public class RunRepository : IRunRepository
    {
        private readonly APIContext _db;

        public RunRepository(APIContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Run>> GetAllRuns()
        {
            var runs = await _db.Runs.Where(r => r.TimeToRun < DateTime.Now).ToListAsync();
            return runs;
        }

        public async Task<Run> GetRun(int id)
        {
            var run = await _db.Runs.FindAsync(id);
            return run;
        }

        public async Task<bool> UpdateRun(Run run)
        {
            _db.Entry(run).State = EntityState.Modified;
            return (await _db.SaveChangesAsync() > 0) ? true : false;
        }
    }
}
