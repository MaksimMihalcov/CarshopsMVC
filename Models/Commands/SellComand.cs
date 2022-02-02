using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    internal class SellCommand: ISellCommand
    {
        public Office office { get; set; }
        public string model { get; set; }
        public uint quantity { get; set; }
        public SellCommand(Office office, string model, uint quantity)
        {
            this.office = office;
            this.model = model;
            this.quantity = quantity;    
        }

        public void Execute()
        {
            office.SellCar(model, quantity);
        }

        public void Undo()
        {
            office.RefaundCar(model, quantity);
        }

        public void Cancellation()
        {
            throw new Exception();
        }
    }
}
