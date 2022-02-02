using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carshops_MVC
{
    [Serializable]
    public class CarItem
    {
        public CarItem() { } //for serialization
        public CarItem(string brand, string model, uint price, uint stockBalance)
        {
            Brand = brand;
            Model = model;
            Price = price;
            StockBalance = stockBalance;
        }
        [Key] public int ItemId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public uint Price { get; set; }
        public uint StockBalance { get; set; }
        public int? OfficeId { get; set; }
        [XmlIgnore] [JsonIgnore] public Office Office { get; set; }
    }
}
