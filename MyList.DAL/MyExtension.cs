namespace MyList.DAL
{
    using System;

    public static class MyExtension
    {
        public static void ListAll(this System.IO.DirectoryInfo dir, Action<Entities.Directory> action)
        {
            Entities.Directory dirRecursive = dir;
            dirRecursive.ListAll(action);
        }

        public static void ListAll(this System.IO.DirectoryInfo dir, Action<Entities.Directory> action, Action<Exception> enqueueException)
        {
            Entities.Directory dirRecursive = dir;
            dirRecursive.ListAll(action, enqueueException);
        }

        public static void ListAllParallel(this System.IO.DirectoryInfo dir, Action<Entities.Directory> action, Action<Exception> enqueueException)
        {
            Entities.Directory dirRecursive = dir;
            dirRecursive.ListAllParallel(action, enqueueException);
        }
    }
}
