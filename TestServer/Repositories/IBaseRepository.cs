using System;

public interface IBaseRepository<T>
{
    Task<T> Get(int id);
}


