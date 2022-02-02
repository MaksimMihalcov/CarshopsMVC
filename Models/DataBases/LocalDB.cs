using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    [Serializable]
    public class LocalDB : IRepository<CarItem>
    {
        public List<CarItem> Items { get ; set; }

        public LocalDB()
        {
            Items = new List<CarItem>();
        }
        public void Create(CarItem item)
        {
            if (Items.Count == 0)
            {
                item.ItemId = 0;
            }
            else
            {
                item.ItemId = Items.Last().ItemId + 1;
            }
            Items.Add(item);
        }

        public void Delete(CarItem item)
        {
            Items.Remove(item);
        }

        public CarItem GetItem(int id)
        {
            return Items.FirstOrDefault(car => car.ItemId == id);
        }

        public List<CarItem> GetItemsList()
        {
            return Items.ToList();
        }

        public void Update(CarItem item)
        {
            var it = Items.FirstOrDefault(car => car.ItemId == item.ItemId);
            if(it != null)
            {
                Items[Items.IndexOf(it)] = item;
            }
        }
    }
}
