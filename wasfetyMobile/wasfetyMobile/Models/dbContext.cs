using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace wasfetyMobile.Models
{
    public class DbContext
    {
        private readonly SQLiteConnection db;
        private readonly SortedSet<int> ids;
        public DbContext(string dbPath)
        {
            db = new SQLiteConnection(dbPath);
            db.CreateTable<AppMealHistory>();
            ids = new SortedSet<int>(from i in db.Table<AppMealHistory>() where i.Email == NetGate.Email select i.Id);
        }
        public async Task<IEnumerable<AppMealHistory>> GetMeals()
        {
            List<AppMealHistory> res = null;
            await Task.Run(async () =>
            {
                res = new List<AppMealHistory>(from i in db.Table<AppMealHistory>() where i.Email == NetGate.Email select i);
                foreach(var item in await new NetGate().GetHistory())
                {
                    if(!ids.Contains(item.Id))
                    {
                        res.Insert(0, item);
                    }
                }
            });
            return res;
        }
        public async void UpdateMeals(params AppMealHistory[] histories)
        {
            await Task.Run(() =>
            {
                foreach (AppMealHistory m in histories)
                {
                    if (!ids.Contains(m.Id))
                    {
                        m.Email = NetGate.Email;
                        ids.Add(m.Id);
                        db.Insert(m);
                    }
                }
            });
        }
    }
}
