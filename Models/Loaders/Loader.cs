using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace Carshops_MVC
{
    public static class Loader
    {
        private static int idCount = 0;
        private static int Id { get { return idCount++;} }
        struct NestedOffice
        {
            public string Brand { get; set; }
            public string Type { get; set; }    
        }
        private static IRepository<CarItem> GetRepository(Office office)
        {
            switch(office.Type)
            {
                case "JSON":
                    {
                        return new LocalDB();
                    }
                case "XML":
                    {
                        return new LocalDB();
                    }
                case "SQL":
                    {
                        return new EFDataBase(office);
                    }
                default:
                    throw new Exception("Type not defined!");
            }
        }
        private static void CreateOfficesLists(List<Office> offices, string type)
        {
            List<Office> typeoffices = offices.Where(office => office.Type == type).ToList();
            switch (type)
            {
                case "JSON":
                    {
                        JsonConverter js = new JsonConverter(typeoffices);
                        js.Deserialize();
                        for(int i = 0; i < typeoffices.Count; i++)
                        {
                            offices[offices.IndexOf(offices.First(o => o.Brand == typeoffices[i].Brand))] = typeoffices[i];
                        }
                    }
                    break;
                case "XML":
                    {
                        XMLConverter xml = new XMLConverter(typeoffices);
                        xml.Deserialize();
                        for (int i = 0; i < typeoffices.Count; i++)
                        {
                            offices[offices.IndexOf(offices.First(o => o.Brand == typeoffices[i].Brand))] = typeoffices[i];
                        }
                    }
                    break;
                case "SQL":
                    {
                        using (EFDataBaseContext db = new EFDataBaseContext())
                        {
                            Office office;
                            for (int i = 0; i < typeoffices.Count; i++)
                            {
                                office = db.Offices.FirstOrDefault(of => of.Brand == typeoffices[i].Brand);
                                if (office == null)
                                {
                                    db.Offices.Add(typeoffices[i]);
                                    db.SaveChanges();
                                }
                            }
                            office = offices.FirstOrDefault(of => of.Type == "SQL");
                            while(office != null)
                            {
                                offices.Remove(office);
                                office = offices.FirstOrDefault(of => of.Type == "SQL");
                            }
                            List<Office> dbList = db.Offices.ToList();
                            int dbCount = db.Offices.Count();
                            idCount = dbList.Last().OfficeId + 1;
                            for (int i = 0; i < dbCount; i++)
                            {
                                dbList[i].CarsDataBase ??= new EFDataBase(dbList[i]);
                                offices.Add(dbList[i]);
                            }
                        }
                    }
                    break;
                default:
                    throw new Exception("Type not defined!");
            }
        }
        private static List<Office> CreateOffices(List<NestedOffice> _no)
        {
            List<NestedOffice> no = _no;
            List<Office> offices = new List<Office>(no.Count);
            for (int i = 0; i < no.Count; i++)
            {
                offices.Add(new Office(no[i].Brand, no[i].Type));
                offices[i].CarsDataBase = GetRepository(offices[i]);
            }
            CreateOfficesLists(offices, "SQL");
            CreateOfficesLists(offices, "JSON");
            CreateOfficesLists(offices, "XML");
            for(int i = 0; i < offices.Count; i++)
            {
                if ((offices[i].Type == "XML") || (offices[i].Type == "JSON"))
                {
                    offices[i].OfficeId = Id;
                }
            }
            return offices;
        }
        private static void SaveOfficesLists(List<Office> offices, string type)
        {
            List<Office> typeoffices = offices.Where(office => office.Type == type).ToList();
            switch (type)
            {
                case "JSON":
                    {
                        JsonConverter js = new JsonConverter(typeoffices);
                        js.Serialize();
                    }
                    break;
                case "XML":
                    {
                        XMLConverter xml = new XMLConverter(typeoffices);
                        xml.Serialize();
                    }
                    break;
                default:
                    throw new Exception("Type not defined!");
            }
        
        }
        public static List<Office> Load()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            return CreateOffices(config.GetRequiredSection("Office").Get<List<NestedOffice>>());
        }
        public static void Save(List<Office> offices)
        {
            SaveOfficesLists(offices, "JSON");
            SaveOfficesLists(offices, "XML");
        }
    }
}
