namespace MyList.DAL.Entities
{
    using System;
    using System.IO;

    public sealed partial class File
    {
        private FileInfo _fileInfo = null;

        private FileInfo FileInfo
        {
            get => _fileInfo ?? new FileInfo(GetFullPathAndName());
            set => _fileInfo = value;
        }

        public Directory Directory { get; private set; }
        public string Drive => Directory.Drive;
        public string Path => Directory.Path;
        public string Name { get; private set; }
        public string Extension { get; private set; }

        public string GetFullName()
            => string.IsNullOrEmpty(Extension) ? Name : $"{Name}.{Extension}";

        public string GetFullPath()
            => Drive + Path;

        public string GetFullPathAndName()
            =>  $"{GetFullPath()}\\{GetFullName()}";

        public int? ContractIndex { get; private set; }

        internal File(Directory directory, string name, string extension, int? contractIndex)
        {
            Directory = directory;
            Name = name;
            Extension = extension;
            ContractIndex = contractIndex;
        }

        private File(FileInfo fileInfo)
        {
            Directory = fileInfo.Directory;
            FileInfo = fileInfo;

            var fullName = FileInfo.Name;
            var extensionIndex = fullName.LastIndexOf('.');

            if (extensionIndex == -1)
            {
                Name = fullName;
                Extension = null;
            }
            else
            {
                Name = fullName.Substring(0, extensionIndex);
                Extension = fullName.Substring(extensionIndex + 1, fullName.Length - extensionIndex - 1);
            }

            ContractIndex = GetContractIndex(Name);
        }

        private File(string fullPathAndName)
        {
            var lastPathIndex = fullPathAndName.LastIndexOf('\\');

            if (lastPathIndex == -1)
                throw new ArgumentException("path don't contains \\");

            var path = fullPathAndName.Substring(0, lastPathIndex);

            Directory = new Directory(path);

            var fullName = fullPathAndName.Substring(lastPathIndex + 1, fullPathAndName.Length - lastPathIndex -1);

            var extensionIndex = fullName.LastIndexOf('.');

            if (extensionIndex == -1)
            {
                Name = fullName;
                Extension = null;
            }
            else
            {
                Name = fullName.Substring(0, extensionIndex);
                Extension = fullName.Substring(extensionIndex + 1, fullName.Length - extensionIndex - 1);
            }

            ContractIndex = GetContractIndex(Name);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is File fileObj))
                return false;
            else
                return GetFullPathAndName().Equals(fileObj.GetFullPathAndName());
        }

        public override int GetHashCode()
            => Drive.GetHashCode() ^ Path.GetHashCode() ^ Name.GetHashCode() ^ Extension.GetHashCode();

        public override string ToString()
            => GetFullPathAndName();

        public static implicit operator File(FileInfo fileInfo)
                => new File(fileInfo);

        public static implicit operator FileInfo(File file)
            => file.FileInfo;

        public static implicit operator Directory(File file)
            => file.Directory;

        public static File Cast(string fullPathAndName)
            => new File(fullPathAndName);

        private static string RemoveCopySintaxIfExist(string name)
        {
            var copy = " - Copy";
            var extensionIndex = name.IndexOf(copy);

            if (extensionIndex == -1)
                return name;

            return name.Remove(extensionIndex, copy.Length);
        }

        public static int? GetContractIndex(string name)
        {
            var nameLessCopy = RemoveCopySintaxIfExist(name);
            var contractIndex = nameLessCopy.LastIndexOf('~');

            if (contractIndex == -1 || contractIndex == 0)
                return null;

            var idxString = nameLessCopy.Substring(contractIndex + 1, nameLessCopy.Length - contractIndex - 1);

            if (int.TryParse(idxString, out int idx))
                return idx;
            else
                return null;
        }
    }
}