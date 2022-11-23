using System;

public interface IBaseRepository<T>
{
    Task<T> Get(int id);
    Task Create(T obj);
}


