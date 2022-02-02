using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    public interface IRepository<T> where T : class
    {
        List<T> GetItemsList(); // получение всех объектов
        T GetItem(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(T item); // удаление объекта по id
    }
}
