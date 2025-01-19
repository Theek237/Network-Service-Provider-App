using NetworkServiceProvider.Entities;
using System.Collections.Generic;

namespace NetworkServiceProvider.Services
{
    public interface IService<T> where T : IEntity
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        bool Delete(int id);
    }
}