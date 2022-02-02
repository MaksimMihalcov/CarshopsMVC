using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    public interface ISerializer<T> where T : class
    {
        public void Serialize();
        public void Deserialize();
    }
}
