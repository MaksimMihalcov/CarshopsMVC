using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    public interface ISellCommand
    {
        void Execute();
        void Cancellation(); 
        void Undo();
    }
}
