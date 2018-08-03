namespace MyList.DAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class Dao<T> : IDao<T>
    {
        public abstract Guid ProcessId { get; }

        public Dao()
        { }

        public abstract void Add(T item);

        public void Add(T item, Action<Exception> enqueueException)
            => this.List(Add, enqueueException);

        public abstract void AddParallel(IEnumerable<T> directories, Action<Exception> enqueueException);

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
