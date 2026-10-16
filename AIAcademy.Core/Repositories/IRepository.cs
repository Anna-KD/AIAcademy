using System;
using System.Collections.Generic;
using AIAcademy.Core.Base;

namespace AIAcademy.Core.Repositories
{
    public interface IRepository<T> where T : RobotTeacher
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
        T? Find(Func<T, bool> predicate);
    }
}
