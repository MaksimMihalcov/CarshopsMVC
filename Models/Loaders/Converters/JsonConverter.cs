using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Carshops_MVC
{
    internal class JsonConverter: ISerializer<Office>
    {
        private const string filename = "cars.json";
        private List<Office> Offices { get; set; }
        public JsonConverter(List<Office> offices)
        {
            Offices = offices;
        }
        public void Serialize()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    List<SerializableOffice> s_office_list = new List<SerializableOffice>();
                    string s = "";
                    for (int i = 0; i < Offices.Count; i++)
                    {
                        s_office_list.Add(new SerializableOffice() { Brand = Offices[i].Brand, SerializableCarsList = Offices[i].CarsDataBase.GetItemsList() });
                    }
                    s = JsonSerializer.Serialize(s_office_list);
                    sw.Write(s);
                }
            }
        }

        public void Deserialize()
        {
            List<SerializableOffice> s_office_list;  
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                string jstring = Encoding.Default.GetString(array);
                try
                {
                    s_office_list = JsonSerializer.Deserialize<List<SerializableOffice>>(jstring);
                }
                catch (Exception)
                {
                    return;
                }
                Office office = null;
                Office bufOffice = null;
                for (int i = 0; i < s_office_list.Count; i++)
                {
                    office = new Office(s_office_list[i].Brand, "JSON") {  CarsDataBase = new LocalDB()};
                    CarItem carItem;
                    for (int j = 0; j < s_office_list[i].SerializableCarsList.Count; j++)
                    {
                        carItem = s_office_list[i].SerializableCarsList[j];
                        //carItem.OfficeId = office.OfficeId;
                        office.CarsDataBase.Create(carItem);
                    }
                    bufOffice = Offices.FirstOrDefault(of => of.Brand == office.Brand);
                    if (bufOffice == null)
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
