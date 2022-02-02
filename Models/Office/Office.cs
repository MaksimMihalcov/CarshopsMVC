using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Carshops_MVC
{
    public class Office
    {
        [Key] public int OfficeId { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        [NotMapped] public IRepository<CarItem> CarsDataBase { get; set; }
        public virtual ICollection<CarItem> Cars { get; set; }
        public virtual ICollection<SparePart> Parts { get; set; }
        public Office(string brand, string type)
        {
            Type = type;
            Cars = new List<CarItem>();
            Parts = new List<SparePart>();
            Brand = brand;
        }
        public void SellCar(string model, uint quantity)
        {
            CarItem car = CarsDataBase?.GetItemsList().FirstOrDefault(car => car.Model == model);
            if ((car != null) && (quantity <= car.StockBalance))
            {
                car.StockBalance -= quantity;
            }
        }
        public void RefaundCar(string model, uint quantity)
        {
            CarItem car = CarsDataBase?.GetItemsList().FirstOrDefault(car => car.Model == model);
            if ((car != null) && (quantity <= car.StockBalance))
            {
                car.StockBalance += quantity;
            }
        }
    }
}
