namespace MyList.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public abstract class Cloud<T> : Dao<T>, IDisposable
    {
        public SqlConnection Connection { get; private set; }

        private readonly Guid _processId;

        public override Guid ProcessId => _processId;

        internal Cloud(Guid processId, SqlConnection connection)
        {
            _processId = processId;
            Connection = connection;
            OpenConnection();
        }

        private Cloud(Guid processId, string connection)
            : this(processId, new SqlConnection(connection))
        { }

        private Cloud(SqlConnection connection)
            : this(Guid.NewGuid(), connection)
        { }

        private Cloud(string connection)
            : this(new SqlConnection(connection))
        { }

        public Cloud()
            : this(@"Persist Security Info=False;User ID=sa;Password=P@ssword09;Initial Catalog=MyCompare1;Data Source=192.168.15.250;MultipleActiveResultSets=True")
        { }

        public Cloud(Guid processId)
            : this(processId, @"Persist Security Info=False;User ID=sa;Password=P@ssword09;Initial Catalog=MyCompare1;Data Source=192.168.15.250;MultipleActiveResultSets=True")
        { } 

        public void OpenConnection() => Connection.Open();

        public void Dispose() => Connection.Close();

        protected void Add(Action<SqlTransaction> add)
        {
            using (var trans = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    add(trans);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        protected void AddParallel<Y>(IEnumerable<Y> items, Action<Y, SqlTransaction> add, Action<Exception> enqueueException)
        {
            using (var trans = Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    void addTransaction(Y localItem)
                        => add(localItem, trans);

                    items.ListParallel(addTransaction, enqueueException);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
