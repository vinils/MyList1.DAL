namespace MyList.DAL.Memories
{
    using MyList;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Directory : Memory<Entities.Directory>, IDirectory
    {
        protected Directory(Guid processId, IEnumerable<Entities.Directory> directories)
            : base(processId, directories)
        { }

        public Directory(Guid processId)
            :base(processId)
        { }

        public Directory()
        { }

        public Directory(IEnumerable<Entities.Directory> directories)
            :base(Guid.NewGuid(), directories)
        { }

        protected Directory(IDirectory directories)
            : base(directories.ProcessId, directories)
        { }

        public void Add(Action<Action<Entities.Directory>, Action<Exception>> list, Action<Exception> enqueueException, out Directory newDirectories)
        {
            Add(list, enqueueException, out IEnumerable<Entities.Directory> directoriesEnumerable);
            newDirectories = new Directory(directoriesEnumerable);
        }

        public void ListFiles(Action<Entities.File> action)
        {
            void listFiles(Entities.Directory directory)
                => directory.ListFiles(action);

            this.List(listFiles);
        }

        public void ListFiles(Action<Entities.File> action, Action<Exception> enqueueException)
        {
            void listFiles(Entities.Directory directory)
                => directory.ListFiles(action, enqueueException);

            this.List(listFiles, enqueueException);
        }

        public void ListFilesParallel(Action<Entities.File> action, Action<Exception> enqueueException)
        {
            void listFiles(Entities.Directory directory) 
                => directory.ListFilesParallel(action, enqueueException);

            this.ListParallel(listFiles, enqueueException);
        }

        public static implicit operator Directory(Clouds.Directory directories)
            => new Directory(directories);

        public static implicit operator Directory(Memories.File files)
        {
            var directoriesEnumerable = files
                .GetDirectories()
                .Distinct();

            return new Directory(files.ProcessId, directoriesEnumerable);
        }

        public static implicit operator Directory(Clouds.File files)
        {
            var directoriesEnumerable = files
                .GetDirectories()
                .Distinct();

            return new Directory(files.ProcessId, directoriesEnumerable);
        }
    }
}
