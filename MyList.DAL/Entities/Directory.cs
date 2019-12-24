namespace MyList.DAL.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public sealed class Directory : IRecursion<Directory>, IRecursion<DirectoryInfo>
    {
        private DirectoryInfo _directoryInfo = null;

        private DirectoryInfo DirectoryInfo
        { get => _directoryInfo ?? new DirectoryInfo(ToString()); set => _directoryInfo = value; }

        public string Drive { get; private set; }
        public string Path { get; private set; }
        public string Name => DirectoryInfo.Name;
        private string GetFullPath() => Drive + Path;

        Directory IRecursion<Directory>.Item
            => DirectoryInfo;

        DirectoryInfo IRecursion<DirectoryInfo>.Item
            => this;

        public IEnumerable<File> FilesEnumerable
        {
            get
            {
                foreach (var fileInfo in DirectoryInfo.GetFiles())
                    yield return fileInfo;
            }
        }

        private Directory(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
            Drive = directoryInfo.Root.FullName.Substring(0, 2);
            Path = (directoryInfo.FullName.Length > 3) ? "\\" + directoryInfo.FullName.Substring(3, directoryInfo.FullName.Length - 3) : "\\";
        }

        internal Directory(string fullPath)
        {
            var driveIndex = fullPath.IndexOf(':');
            Drive = fullPath.Substring(0, driveIndex + 1);
            Path = fullPath.Substring(driveIndex + 1, fullPath.Length - driveIndex - 1);
        }

        public void ListAll(Action<Directory> action)
        {
            IRecursion<Directory> recursion = this;
            recursion.ListAll(action);
        }

        public void ListAll(Action<Directory> action, Action<Exception> enqueueException)
        {
            IRecursion<Directory> recursion = this;
            recursion.ListAll(action, enqueueException);
        }

        public void ListAllParallel(Action<Directory> action, Action<Exception> enqueueException)
        {
            IRecursion<Directory> recursion = this;
            recursion.ListAllParallel(action, enqueueException);
        }

        public void ListFiles(Action<File> action)
            => FilesEnumerable.List(action);

        public void ListFiles(Action<File> action, Action<Exception> enqueueException)
            => FilesEnumerable.List(action, enqueueException);

        public void ListFilesParallel(Action<File> action, Action<Exception> enqueueException)
            => FilesEnumerable.ListParallel(action, enqueueException);

        IEnumerator<IRecursion<Directory>> IEnumerable<IRecursion<Directory>>.GetEnumerator()
        {
            foreach (var dir in (IEnumerable<Directory>)this)
                yield return dir;
        }

        public IEnumerator<Directory> GetEnumerator()
        {
            foreach (var dir in (IEnumerable<DirectoryInfo>)this)
                yield return dir;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<IRecursion<DirectoryInfo>> IEnumerable<IRecursion<DirectoryInfo>>.GetEnumerator()
        {
            foreach (var dir in (IEnumerable<Directory>)this)
                yield return dir;
        }

        IEnumerator<DirectoryInfo> IEnumerable<DirectoryInfo>.GetEnumerator()
        {
            foreach (var dir in DirectoryInfo.GetDirectories())
                yield return dir;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Directory directoryObj))
                return false;
            else
            {
                var thisFullPath = GetFullPath();
                var directoryObjFullPath = directoryObj.GetFullPath();
                return thisFullPath.Equals(directoryObjFullPath);
            }
        }

        public override int GetHashCode() 
            => Drive.GetHashCode() ^ Path.GetHashCode();

        public override string ToString() 
            => GetFullPath();

        public static implicit operator Directory(DirectoryInfo directoryInfo)
            => new Directory(directoryInfo);

        public static implicit operator DirectoryInfo(Directory directory)
            => directory.DirectoryInfo;

        public static explicit operator string(Directory directory)
            => directory.ToString();
    }
}
