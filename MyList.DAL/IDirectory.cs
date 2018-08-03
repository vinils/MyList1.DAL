namespace MyList.DAL
{
    using System;
    using Entities;

    public interface IDirectory : IDao<Directory>
    {
        void ListFiles(Action<File> action);
        void ListFilesParallel(Action<File> action, Action<Exception> enqueueException);
    }
}