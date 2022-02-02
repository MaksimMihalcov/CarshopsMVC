using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Carshops_MVC
{
    [Serializable]
    public class SparePart
    {
        public SparePart() { } //for serialization
        public SparePart(string name, uint key) 
        { 
            Name = name;
            Key = key;
        }
        [Key] public int PartId { get; set; }
        public string Name { get; set; }
        public uint Key { get; set; }
        public int? OfficeId { get; set; }
        [XmlIgnore] [JsonIgnore] public Office Office { get; set; }
    }
}
