using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.InMemory
{
    public interface IGenericRepository<T> where T : IdObject
    {
        void Add(T entity);
        void Delete(Guid id);
        void DeleteAll();
        T Get(Guid id);
        IEnumerable<T> GetAll();
        void Update(T entity);
    }
}
