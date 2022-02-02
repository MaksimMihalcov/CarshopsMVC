using System;
using System.Collections.Generic;
using System.Linq;

namespace Carshops_MVC
{
    public class MainOffice: IDisposable
    {
        public List<Office> Dealerships { get; private set; }
        private OrderProcessor op;
        public MainOffice(uint transactionCount)
        {
            op = OrderProcessor.GetInstance(transactionCount);
            Dealerships = Loader.Load();
        }
        public void BuyCars(string brand, string model, uint quantity)
        {
            Office office = Dealerships.FirstOrDefault(office => office.Brand == brand);
            CarItem car = office?.CarsDataBase?.GetItemsList().FirstOrDefault(car => car.Model == model); 
            if ((office != null) && (car != null) && (quantity <= car.StockBalance))
            {
                op.BuyCars(office, car.Model, quantity);
            }
        }

        public void Dispose()
        {
            Loader.Save(Dealerships);
        }

        public void RefaundCars(string brand, string model, uint quantity)
        {
            Office office = Dealerships.FirstOrDefault(office => office.Brand == brand);
            CarItem car = office.CarsDataBase.GetItemsList().FirstOrDefault(car => car.Model == model);
            if ((office != null) && (car != null))
            {
                op.RefaundCars(office, car.Model, quantity);
            }
        }
    }
}
