namespace MyList.DAL.Memories
{
    using MyList;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class File : Memory<Entities.File>, IEnumerable<FileInfo>, IFile
    {
        public IEnumerable<Entities.Directory> GetDirectories()
            => _items.Select(f => (Entities.Directory)f);

        protected File(Guid processId, IEnumerable<Entities.File> files)
            : base(processId, files)
        { }

        public File(Guid processId)
            : base(processId)
        { }

        public File()
            : base()
        { }

        public File(IEnumerable<Entities.File> files)
            : base(Guid.NewGuid(), files)
        { }

        protected File(IFile files)
            : base(files.ProcessId, files)
        { }

        internal void Add(FileInfo fileInfo)
            => _items.Add(fileInfo);

        public void Add(Action<Action<Entities.File>, Action<Exception>> list, Action<Exception> enqueueException, out File newFiles)
        {
            Add(list, enqueueException, out IEnumerable<Entities.File> filesEnumerable);
            newFiles = new File(filesEnumerable);
        }

        IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
            => ((IEnumerable<FileInfo>)_items).GetEnumerator();

        public static implicit operator File(Clouds.File files)
            => new File(files);

        public static File Cast(IEnumerable<FileInfo> files, Action<Exception> enqueueException)
        {
            var ret = new File();
            files.List(ret.Add, enqueueException);
            return ret;
        }

        public static File CastParallel(IEnumerable<FileInfo> files, Action<Exception> enqueueException)
        {
            var ret = new File();
            files.ListParallel(ret.Add, enqueueException);
            return ret;
        }

        public static File Cast(IEnumerable<FileInfo> files)
        {
            var ret = new File();
            files.List(ret.Add);
            return ret;
        }
    }
}
