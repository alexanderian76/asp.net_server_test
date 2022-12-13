using System;

public interface IBaseEditableRepository<T>: IBaseRepository<T>
{
    Task Create(T obj);
}


