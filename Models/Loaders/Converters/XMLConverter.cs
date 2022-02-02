using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Carshops_MVC
{
    public class XMLConverter: ISerializer<Office>
    {
        private const string filename = "cars.xml";
        private readonly XmlSerializer serializer;  
        private List<Office> Offices { get; set; }
        public XMLConverter(List<Office> offices)
        {
            serializer = new XmlSerializer(typeof(SerializableMOffice));
            Offices = offices;  
        }
        public void Serialize()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                SerializableMOffice s_moffice = new SerializableMOffice();
                for (int i = 0; i < Offices.Count; i++)
                {
                    s_moffice.SerializableOfficeList.Add(new SerializableOffice() { Brand = Offices[i].Brand, SerializableCarsList = Offices[i].CarsDataBase.GetItemsList() });
                }
                serializer.Serialize(fs, s_moffice);
            }
        }
        public void Deserialize()
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                SerializableMOffice s_moffice;
                try
                {
                    s_moffice = (SerializableMOffice)serializer.Deserialize(fs);
                }
                catch (Exception)
                {
                    return;
                }
                Office office = null;
                Office bufOffice = null;
                for (int i = 0; i < s_moffice.SerializableOfficeList.Count; i++)
                {
                    office = new Office(s_moffice.SerializableOfficeList[i].Brand, "XML") {  CarsDataBase = new LocalDB()};
                    CarItem carItem;
                    for (int j = 0; j < s_moffice.SerializableOfficeList[i].SerializableCarsList.Count; j++)
                    {
                        carItem = s_moffice.SerializableOfficeList[i].SerializableCarsList[j];
                        //carItem.OfficeId = office.OfficeId;
                        office.CarsDataBase.Create(carItem);
                    }
                    bufOffice = Offices.FirstOrDefault(of => of.Brand == office.Brand);
                    if(bufOffice == null)
                    {
                        Offices.Add(office);
                    }
                    else
                    {
                        Offices[Offices.IndexOf(bufOffice)] = office;
                    }
                }
            }
        }
    }
}
