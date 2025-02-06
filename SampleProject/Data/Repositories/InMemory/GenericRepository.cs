using Common;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories.InMemory
{
    //Registering as singleton because it is in memory
    //For Databases we will make it Scoped
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class GenericRepository<T> : IGenericRepository<T> where T : IdObject
    {
        protected readonly List<T> _items = new List<T>();

        public void Add(T entity)
        {
            _items.Add(entity);
        }

        public void Delete(Guid id)
        {
            var entity = _items.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _items.Remove(entity);
            }
        }

        public void DeleteAll()
        {
            _items.Clear(); 
        }

        public T Get(Guid id)
        {
            return _items.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public void Update(T entity)
        {            
            var existingEntity = Get(entity.Id);
            if (existingEntity != null)
            {
                int index = _items.IndexOf(existingEntity);
                _items[index] = entity;
            }
        }
    }
}
