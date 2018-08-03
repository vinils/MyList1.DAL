namespace MyList.DAL.Clouds
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    public class File : Cloud<Entities.File>, IEnumerable<FileInfo>, IFile
    {
        public IEnumerable<Entities.Directory> GetDirectories()
            => ((IEnumerable<Entities.File>)this).Select(f => (Entities.Directory)f);

        public File()
        { }

        public File(Guid processId)
            : base(processId)
        { }

        public override void Add(Entities.File file)
        {
            void localAdd(SqlTransaction trans) 
                => ExecuteNonQuery(ProcessId, Connection, file, trans);

            Add(localAdd);
        }

        internal void Add(IEnumerable<Entities.Directory> directories, Action<Exception> enqueueException)
        {
            Action<Action<Entities.Directory>, Action<Exception>> action = directories.List;
            action.JoinEnqueueException(Add, enqueueException);
        }

        public override void AddParallel(IEnumerable<Entities.File> files, Action<Exception> enqueueException)
        {
            void addTransaction(Entities.File file, SqlTransaction trans)
                => ExecuteNonQuery(ProcessId, Connection, file, trans);

            AddParallel(files, addTransaction, enqueueException);
        }

        public int ExecuteNonQuery(Guid processId, SqlConnection connection, Entities.File file, SqlTransaction transaction)
        {
            var query1 = "INSERT INTO [MyCompare1].[dbo].[Files] (ProcessId, Drive, Path, Name, Extension, ContractIndex) VALUES (@ProcessId, @Drive, @Path, @Name, @Extension, @ContractIndex)";
            using (var command = new SqlCommand(query1, connection, transaction))
            {
                object extension = DBNull.Value;

                if (!string.IsNullOrEmpty(file.Extension))
                    extension = file.Extension;

                object contractIndex = DBNull.Value;

                if (file.ContractIndex.HasValue)
                    contractIndex = file.ContractIndex.Value;

                command.Parameters.AddWithValue("@ProcessId", processId);
                command.Parameters.AddWithValue("@Drive", file.Drive);
                command.Parameters.AddWithValue("@Path", file.Path);
                command.Parameters.AddWithValue("@Name", file.Name);
                command.Parameters.AddWithValue("@Extension", extension);
                command.Parameters.AddWithValue("@ContractIndex", contractIndex);

                return command.ExecuteNonQuery();
            }
        }

        public override IEnumerator<Entities.File> GetEnumerator()
        {
            var ds = new System.Data.DataTable();

            using (var trans = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var query = "SELECT * FROM Files (NOLOCK) where ProcessId = '" + ProcessId + "'";
                    using (var command = new SqlCommand(query, Connection, trans))
                    {
                        var da = new SqlDataAdapter { SelectCommand = command };

                        da.Fill(ds);
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }


            for (var x = 0; x <= ds.Rows.Count -1; x++)
            {
                var drive = ds.Rows[x]["Drive"].ToString();
                var path = ds.Rows[x]["Path"].ToString();
                var name = ds.Rows[x]["Name"].ToString();
                var extension = ds.Rows[x]["Extension"].ToString();
                int? contractIndex = null;

                if (!string.IsNullOrEmpty(ds.Rows[x]["ContractIndex"].ToString()))
                    contractIndex = int.Parse(ds.Rows[x]["ContractIndex"].ToString());

                var directory = new Entities.Directory(drive + path);

                yield return new Entities.File(directory, name, extension, contractIndex);
            }
        }

        IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
        {
            foreach (var file in this)
                yield return file;
        }
    }
}
