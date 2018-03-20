using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Distrubutor.Core.Data
{
    public interface IRepository<T>
    {
        T GetByID(object id);
        void Insert(T entity);
        void Insert(IEnumerable<T> entity);
        void Update(T entity);
        void Update(IEnumerable<T> entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entity);
    }
}
