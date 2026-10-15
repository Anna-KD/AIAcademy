using System;
using System.Collections.Generic;
using System.Linq;
using AIAcademy.Core.Base;

namespace AIAcademy.Core.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : RobotTeacher
    {
        private readonly List<T> _items = new();

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public T? Find(Func<T, bool> predicate)
        {
            return _items.FirstOrDefault(predicate);
        }
    }
}
