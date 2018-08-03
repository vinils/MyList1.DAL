namespace MyList.DAL.Clouds
{
    using MyList;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;

    public class Directory : Cloud<Entities.Directory>, IEnumerable<DirectoryInfo>, IDirectory
    {
        public Directory()
        { }

        public Directory(Guid processId)
            : base(processId)
        { }

        public override void Add(Entities.Directory direcotry)
        {
            void localAdd(SqlTransaction trans) 
                => ExecuteNonQuery(ProcessId, Connection, direcotry, trans);

            Add(localAdd);
        }

        public override void AddParallel(IEnumerable<Entities.Directory> directories, Action<Exception> enqueueException)
        {
            void addTransaction(Entities.Directory directory, SqlTransaction trans)
                => ExecuteNonQuery(ProcessId, Connection, directory, trans);

            AddParallel(directories, addTransaction, enqueueException);
        }

        public void ListFiles(Action<Entities.File> action)
        {
            void listFiles(Entities.Directory directory)
                => directory.ListFiles(action);
            IEnumerable<Entities.Directory> enumerable = this;

            enumerable.List(listFiles);
        }

        public void ListFilesParallel(Action<Entities.File> action, Action<Exception> enqueueException)
        {
            void listFiles(Entities.Directory directory)
                => directory.ListFilesParallel(action, enqueueException);
            IEnumerable<Entities.Directory> enumerable = this;

            enumerable.ListParallel(listFiles, enqueueException);
        }

        protected int ExecuteNonQuery(Guid processId, SqlConnection connection, Entities.Directory directory, SqlTransaction transaction)
        {
            var query1 = "INSERT INTO [MyCompare1].[dbo].[Directories] (ProcessId, Path) VALUES (@ProcessId, @Path)";
            using (var command = new SqlCommand(query1, connection, transaction))
            {
                command.Parameters.AddWithValue("@ProcessId", processId);
                command.Parameters.AddWithValue("@Path", (string)directory);

                return command.ExecuteNonQuery();
            }
        }

        public override IEnumerator<Entities.Directory> GetEnumerator()
        {
            var ds = new System.Data.DataTable();

            using (var trans = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var query = "SELECT * FROM Directories where ProcessId = '" + ProcessId + "'";
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
                var path = ds.Rows[x]["Path"].ToString();

                yield return new Entities.Directory(path);
            }
        }

        IEnumerator<DirectoryInfo> IEnumerable<DirectoryInfo>.GetEnumerator()
        {
            foreach (var dir in this)
                yield return dir;
        }
    }
}
