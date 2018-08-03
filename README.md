# MyList.DAL

This project was created as part of [MyList1](https://github.com/vinils/MyList1) to be a persistence layer to store the data in different places, such as memory or SQL

## How to use:

```csharp
using MyList.DAL;

public void Test1()
{
    var directories = new Memories.Directory();
    var directory = new System.IO.DirectoryInfo("U:\\");
    directory.ListAll(directories.Add);
}
```
or
```csharp
    var directories = new Clouds.Directory();
    var directory = new System.IO.DirectoryInfo("U:\\");
    directories.Add(directory);
```
or
```csharp
    var exceptions = new ConcurrentBag<Exception>();
    directory.ListAll(directories.Add, exceptions.Add);
```
or
```csharp
    directory.ListAllParallel(directories.Add, exceptions.Add);
```
