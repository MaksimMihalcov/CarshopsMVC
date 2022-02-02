using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Carshops_MVC
{
    [Serializable]
    [XmlRoot("SerializableMOffice")]
    public class SerializableMOffice
    {
        [XmlArray("SerializableOfficeList"), XmlArrayItem(typeof(SerializableOffice), ElementName = "SerialazibleOffice")]
        public List<SerializableOffice> SerializableOfficeList { get; set; }
        public SerializableMOffice()
        {
            SerializableOfficeList = new List<SerializableOffice>();
        }    
    }
}
