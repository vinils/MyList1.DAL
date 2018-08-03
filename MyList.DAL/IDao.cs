namespace MyList.DAL
{
    using System;
    using System.Collections.Generic;

    public interface IDao<T> : IEnumerable<T>
    {
        Guid ProcessId { get; }

        void Add(T item);
        void AddParallel(IEnumerable<T> directories, Action<Exception> enqueueException);
    }
}