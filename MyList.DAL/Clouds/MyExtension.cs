namespace MyList.DAL.Clouds
{
    using System;
    using System.Collections.Generic;

    public static class MyExtension
    {
        public static void SaveOnCloud(
            this Memories.File files, 
            IEnumerable<Entities.File> newFiles, 
            Action<Exception> enqueueException)
        {
            using (var filesCloud = new File(files.ProcessId))
            {
                filesCloud.AddParallel(newFiles, enqueueException);
            }
        }

        public static void SaveOnCloud(this Memories.File files, Action<Exception> enqueueException)
            => files.SaveOnCloud(files, enqueueException);

        public static void SaveOnCloud(
            this Memories.Directory directories, 
            IEnumerable<Entities.Directory> newDirectories, 
            Action<Exception> enqueueException)
        {
            using (var filesCloud = new Directory(directories.ProcessId))
            {
                filesCloud.AddParallel(newDirectories, enqueueException);
            }
        }

        public static void SaveOnCloud(this Memories.Directory directories, Action<Exception> enqueueException)
            =>  directories.SaveOnCloud(directories, enqueueException);
    }
}
