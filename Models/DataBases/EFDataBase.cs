using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Carshops_MVC
{
    public class EFDataBase : IRepository<CarItem>
    { 
        private Office Office { get; set; }
        public EFDataBase(Office office)
        {
            Office = office;
        }

        public void Create(CarItem item)
        {
            using (EFDataBaseContext db = new EFDataBaseContext())
            {
                if (item != null)
                {
                    Office office = db.Offices.FirstOrDefault(of => of.Brand == Office.Brand);
                    if (office != null)
                    {
                        office.Cars.Add(item);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void Delete(CarItem item)
        {
            CarItem carItem;
            using (EFDataBaseContext db = new EFDataBaseContext())
            {
                if (item != null)
                {
                    carItem = db.CarItems.FirstOrDefault(c => c.Model == item.Model && c.Brand == item.Brand);
                    if (carItem != null)
                    {
                        db.CarItems.Remove(carItem);
                        db.SaveChanges();
                    }
                }
            }
        }

        public CarItem GetItem(int id)
        {
            using (EFDataBaseContext DataBase = new EFDataBaseContext())
            {
                return DataBase.CarItems.Include(item => item.Office).FirstOrDefault(item => item.ItemId == id);
            }
        }

        public List<CarItem> GetItemsList()
        {
            using (EFDataBaseContext DataBase = new EFDataBaseContext())
            {
                return DataBase.CarItems.Where(c => c.Brand == Office.Brand).ToList();
            }
        }

        public void Update(CarItem item)
        {
            using (EFDataBaseContext DataBase = new EFDataBaseContext())
            {
                DataBase.CarItems.Update(item);
                DataBase.SaveChanges();
            }
        }
    }
}
