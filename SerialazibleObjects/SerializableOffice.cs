using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Carshops_MVC
{
    [Microsoft.EntityFrameworkCore.Keyless]
    [Serializable]
    public class SerializableOffice
    {
        public string Brand { get; set; }
        [XmlArray("SerializableCarsList"), XmlArrayItem(typeof(CarItem), ElementName = "CarItem")]
        public List<CarItem> SerializableCarsList { get; set; }
        //[XmlArray("Parts"), XmlArrayItem(typeof(SparePart), ElementName = "Part")]
        //public List<SparePart> Parts { get; set; }
        public SerializableOffice()
        {
            SerializableCarsList = new List<CarItem>(); 
        }
    }
}
