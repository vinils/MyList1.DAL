namespace MyList.DAL
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public abstract class Memory<T> : Dao<T>, IEnumerable<T>
    {
        public static Guid CastHashCodeToGuid(int hashCode)
        {   
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(hashCode).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static Guid CastHashCodeToGuid(Object obj)
        {
            return CastHashCodeToGuid(obj.GetHashCode());
        }

        private readonly Guid _processId;

        public override Guid ProcessId => _processId;

        protected ConcurrentBag<T> _items
            = new ConcurrentBag<T>();

        public int Count
            => _items.Count;

        protected Memory(Guid processId, IEnumerable<T> items)
        {
            _processId = processId;
            _items = new ConcurrentBag<T>(items);
        }

        public Memory(Guid processId)
            => _processId = processId;

        public Memory(IEnumerable<T> items)
            :this(Guid.NewGuid(), items)
        { }

        public Memory()
            => _processId = CastHashCodeToGuid(_items.GetHashCode());

        public override void Add(T item)
            => _items.Add(item);

        protected void Add(Action<Action<T>, Action<Exception>> list, Action<Exception> enqueueException, out IEnumerable<T> newItems)
        {
            var newItemsRet = new ConcurrentBag<T>();

            void addAndLoadNewItems(T item)
            {
                Add(item);
                newItemsRet.Add(item);
            }

            list(addAndLoadNewItems, enqueueException);

            newItems = newItemsRet;
        }

        public override void AddParallel(IEnumerable<T> items, Action<Exception> enqueueException)
            => items.ListParallel(_items.Add, enqueueException);

        public override IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)_items).GetEnumerator();
    }
}
