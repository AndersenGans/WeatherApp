using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp_OKopot.DAL.EF;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.DAL.Repositories
{
    class HistoryRepository:IRepository<History>
    {
        private WeatherDbContext dbContext;

        public HistoryRepository(WeatherDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<History> GetAll()
        {
            return dbContext.Histories;
        }

        public History Get(int id)
        {
            return dbContext.Histories.Find(id);
        }

        public void Create(History history)
        {
            dbContext.Histories.Add(history);
        }

        public void Update(History history)
        {
            dbContext.Entry(history).State = EntityState.Modified;
        }

        public IEnumerable<History> Find(Func<History, Boolean> predicate)
        {
            return dbContext.Histories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            History history = dbContext.Histories.Find(id);
            if (history != null)
                dbContext.Histories.Remove(history);
        }

        public void Delete(IEnumerable<History> histories)
        {
            if (histories.Any())
                dbContext.Histories.RemoveRange(histories);
        }
    }
}
